using Blog.Dtos.Users;
using Blog.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repositories.Users
{
    public interface IUserRepository
    {

        Task<bool> EmailIfExistsAsync (string email);
        Task<bool> MobileNumerIfExists(string number);
        void CreateUserAsync(UserDto user);
        User GetById(int id);
        User GetByEmail(string email);
    }
}
