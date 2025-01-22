using Blog.Dtos.Users;
using Blog.Models;
using System.Threading.Tasks;

namespace Blog.Services.UserService
{
    public interface IUserService
    {
        Task<Result<UserDto>> GetUser(string username);
    }
}
