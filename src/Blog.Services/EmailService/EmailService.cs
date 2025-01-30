using Blog.Dtos.Email;
using Blog.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System;

namespace Blog.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task<Result<string>> SendEmailAsync(EmailDto emailDto)
        {
            try
            {
                using var message = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = emailDto.Subject,
                    Body = emailDto.Body,
                    IsBodyHtml = emailDto.IsHtml
                };

                message.To.Add(new MailAddress(emailDto.To));

                using var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword)
                };

                await smtpClient.SendMailAsync(message);
                _logger.LogInformation($"Email sent successfully to {emailDto.To}");
                return Result<string>.Success("Verification Email has been sent");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email: {ex.Message}");
                return Result<string>.Success("Failed Verification Email");
            }
        }
    }
}
