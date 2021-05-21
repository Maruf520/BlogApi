using Blog.Dtos.Users;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegisterDto userDto);
        Task<ServiceResponse<string>> Login(UserLoginDto user);
    }
}
