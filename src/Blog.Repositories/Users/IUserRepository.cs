﻿using Blog.Dtos.Users;
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

        Task<bool> CheckExistsByEmail(string email, string mobile);
        void CreateUserAsync(UserDto user);
        User GetUserById(int id);
        User GetByEmail(string email);
    }
}
