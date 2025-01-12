using AutoMapper;
using Blog.Dtos.Users;
using Blog.Models;
using Blog.Models.UserModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper mapper;
        public UserRepository(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.mapper = mapper;
            _userManager = userManager;
        }
        public async Task<bool> EmailIfExistsAsync(string email)
        {
            if (_context.Users.Any(x => x.Email == email))
            {
                return true;
            }
            return false;
        }

        public async Task CreateUserAsync(UserRegisterDto userDto)                                                     
        {
                var user = mapper.Map<ApplicationUser>(userDto);
                user.Id = Guid.NewGuid();
                user.UserName = user.Email;
              var result =  await _userManager.CreateAsync(user, userDto.Password);

         
        }
        public async Task<ApplicationUser> GetById(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.ToString().ToLower() == id.ToString().ToLower());
            return user;
        }
        public ApplicationUser GetByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(c => c.Email == email);
            return user;
        }

        public async Task<bool> MobileNumerIfExists (string mobile)
        {
            if (_context.Users.Any(x => x.Mobile == mobile))
            {

                return true;
            }
            else
                return false;
        }
    }
}
