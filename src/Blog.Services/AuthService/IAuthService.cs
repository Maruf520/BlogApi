using Blog.Dtos.Users;
using Blog.Models;
using System.Threading.Tasks;

namespace Blog.Services.AuthService
{
    public interface IAuthService
    {
        Task<Result<LoginResponse>> Login(UserLoginDto userDto);
        Task<Result<string>> Register(UserRegisterDto user);
    }
}
