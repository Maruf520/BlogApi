﻿using AutoMapper;
using Blog.Dtos.Users;
using Blog.Models;
using Blog.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly BlogContext context;
        private readonly IMapper mapper;
        public UserRepository(BlogContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<bool> CheckExistsByEmail(string email, string mobile)
        {
            if (context.Users.Any(x => x.Mobile.ToLower() == mobile.ToLower() || x.Email == email))
            {
                return true;
            }
            return false;
        }

        public void CreateUserAsync(UserDto user)
        {
            var usertomap = mapper.Map<User>(user);
            context.Users.Add(usertomap);
            context.SaveChanges();
        }
        public User GetUserById(int id)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == id);
            return user;
        }
        public User GetByEmail(string email)
        {
            var user = context.Users.FirstOrDefault(c => c.Email == email);
            return user;
        }
    }
}
