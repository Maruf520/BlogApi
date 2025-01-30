using Blog.Dtos.Email;
using Blog.Models;
using System.Threading.Tasks;

namespace Blog.Services.EmailService
{
    public interface IEmailService
    {
        Task<Result<string>> SendEmailAsync(EmailDto emailDto);
    }
}
