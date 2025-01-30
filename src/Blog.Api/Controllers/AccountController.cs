using Blog.Api.Content;
using Blog.Dtos.Account;
using Blog.Dtos.Email;
using Blog.Models;
using Blog.Services.AuthService;
using Blog.Services.EmailService;
using Blog.Services.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailSender;
        public AccountController(IUserService userService, IAuthService authService, IEmailService emailSender)
        {
            _userService = userService;
            _authService = authService;
            _emailSender = emailSender;
        }
        [HttpGet]
        public async Task<IActionResult> GetUser([FromQuery] string email)
        {
            var user = await _userService.GetUser(email);
            return user.IsSuccess ? Ok(user.Data) : BadRequest(user.Error.Message);
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDto dto)
        {
            var user = await _userService.GetUser(dto.Email);
            if (user.IsFailure == true)
            {
                return BadRequest("User Not Found. Please contact with admin.");
            }

            var token = await _authService.GeneratePasswordToken(dto.Email);
            if(token.Data != null)
            {
                var emailBody = EmailTemplates.GeneratePasswordResetEmail($"{user.Data.FirstName + " " + user.Data.LastName}", token.Data);
                var emailDto = new EmailDto();
                emailDto.Body = emailBody;
                emailDto.To = dto.Email;
                emailDto.Subject = "Password Reset URL";
                emailDto.IsHtml = true;
                var result = await _emailSender.SendEmailAsync(emailDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result.Error.Message);
            }
            return token.IsSuccess ? Ok(token) : BadRequest(token.Error.Message);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody]ConfirmEmail confirmEmail)
        {
            if (string.IsNullOrEmpty(confirmEmail.Email) || string.IsNullOrEmpty(confirmEmail.Token))
                return BadRequest("Invalid email confirmation link");

            var confirmation = await _authService.ConfirmEmailAsync(confirmEmail);

            return confirmation.IsSuccess ? Ok(confirmation.Data) : BadRequest(confirmation.Error.Message);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto resetDto)
        {
            var verify = await _authService.ResetPassword(resetDto.Email, resetDto.Token, resetDto.NewPassword);

            if (!verify)
            {
                return BadRequest("Invalid Token");
            }
            return Ok("Password Has been reset successfully.");
        }
        private static string GetRequestScheme(bool isHttps)
        {
            return isHttps ? "https" : "http";
        }
    }
}
