using AutoMapper;
using Blog.Dtos.Users;
using Blog.Models;
using Blog.Models.UserModel;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Blog.Services.UserService
{
    public class UserService: IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Result<UserDto>> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
            {
                return Result<UserDto>.Failure("User Not Found. Please contact with admin.");
            }


            var maskedEmail = string.Format("{0}****{1}", user.Email[0],
            user.Email.Substring(user.Email.IndexOf('@') - 1));

            var result = _mapper.Map<UserDto>(user); 
            result.Email = maskedEmail;
            
            return Result<UserDto>.Success(result);
        }

        public async Task<Result<ApplicationUser>> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return Result<ApplicationUser>.Failure("User Not Found");
            }

            return Result<ApplicationUser>.Success(user);
        }
    }
}
