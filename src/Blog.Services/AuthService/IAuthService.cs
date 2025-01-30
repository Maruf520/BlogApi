using Blog.Dtos.Email;
using Blog.Dtos.Users;
using Blog.Models;
using Blog.Models.UserModel;
using System.Threading.Tasks;

namespace Blog.Services.AuthService
{
    public interface IAuthService
    {
        Task<Result<LoginResponse>> Login(UserLoginDto userDto);
        Task<Result<ApplicationUser>> Register(UserRegisterDto user);
        Task<Result<UserDto>> GetUser(string userId);
        Task<Result<string>> GeneratePasswordToken(string email);
        Task<bool> ResetPassword(string email, string token, string newPassword);
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser applicationUser);
        Task<Result<string>> ConfirmEmailAsync(ConfirmEmail confirmEmail);
        Task<Result<UserUpdateDto>> UpdateUser(UserUpdateDto dto);
    }
}
