using AutoMapper;
using Blog.Dtos.Users;
using Blog.Models;
using Blog.Repositories.Users;
using Blog.Services.UserExtentionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IUserExtentionService userExtentionService;
        public AuthService(IUserRepository userRepository, IUserExtentionService userExtentionService, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.userExtentionService = userExtentionService;
            this.mapper = mapper;
        }

        public async Task<ServiceResponse<string>> Login(UserLoginDto user)
        {
            ServiceResponse<string> response = new();
            var userToGet = userRepository.GetByEmail(user.Email);
            if (userToGet == null)
            {
                response.Success = false;
                response.Message = "User Not Found";
                return response;
            }
            else if (!VerifyPasswordHash(user.Password, userToGet.PasswordHash, userToGet.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wromg Password";
            }
            else
            {
                response.Data = await userExtentionService.GenerateAccessToken(userToGet);
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(UserRegisterDto user)
        {
            ServiceResponse<int> response = new();
            if (userRepository.EmailIfExistsAsync(user.Email).Result == true)
            {
                response.Success = false;
                response.Message = "Email Already Exists";
                return response;
            }
            else if (userRepository.MobileNumerIfExists(user.Mobile).Result == true)
            {
                response.Success = false;
                response.Message = "Phone Number Already Exists";
                return response;
            }
            else if (userRepository.EmailIfExistsAsync(user.Email).Result == true && userRepository.MobileNumerIfExists(user.Mobile).Result == true)
            {
                response.Success = false;
                response.Message = "Email & Phone number Already Exists";
                return response;
            }
            
            UserDto userDto = new UserDto();
            var userToCreate = mapper.Map<UserDto>(user);
            CreateHashPassword(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            userToCreate.PasswordHash = passwordHash;
            userToCreate.PasswordSalt = passwordSalt;

            userRepository.CreateUserAsync(userToCreate);
            response.Success = true;
            response.Message = "Regsitered Successsyfully";
            return response;
        }


        private void CreateHashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        private bool VerifyPasswordHash(string password, byte[] passswordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (computeHash[i] != passswordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
