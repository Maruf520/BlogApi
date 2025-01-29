using Blog.Dtos.Email;
using System.Threading.Tasks;

namespace Blog.Services.EmailService
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailDto emailDto);
    }
}
