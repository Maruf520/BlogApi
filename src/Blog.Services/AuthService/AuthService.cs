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

        public async Task<Result<LoginResponse>> Login(UserLoginDto userDto)
        {
            var userToGet = userRepository.GetByEmail(userDto.Email);
            if (userToGet == null)
            {
                return Result<LoginResponse>.Failure("Email not found.");
            }
            else if (!VerifyPasswordHash(userDto.Password, userToGet.PasswordHash, userToGet.PasswordSalt))
            {
                return Result<LoginResponse>.Failure("Wromg Password");
            }
           var token = await userExtentionService.GenerateAccessToken(userToGet);
           var result = userRepository.GetByEmail(userDto.Email);
            var response = new LoginResponse();

            response.Token = token;
            response.ExpiresInSeconds = 3600;
            response.User.FirstName = result.FirstName;
            response.User.FirstName = result.FirstName;
            response.User.LastName = result.LastName;
            response.User.Mobile = result.Mobile;
            response.User.Address = result.Address;
            response.User.Image = result.Image;
            response.User.Email = result.Email;

            return Result<LoginResponse>.Success(response);
        }

        public async Task<Result<string>> Register(UserRegisterDto user)
        {

            if (userRepository.EmailIfExistsAsync(user.Email).Result == true)
            {
                return Result<string>.Failure("Email Already Exists.");
            }
            else if (userRepository.MobileNumerIfExists(user.Mobile).Result == true)
            {
                return Result<string>.Failure("Mobile Number already Exists.");
            }
            else if (userRepository.EmailIfExistsAsync(user.Email).Result == true && userRepository.MobileNumerIfExists(user.Mobile).Result == true)
            {
                return Result<string>.Failure("Email already Exists.");

            }

            UserDto userDto = new UserDto();
            var userToCreate = mapper.Map<UserDto>(user);
            CreateHashPassword(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            userToCreate.PasswordHash = passwordHash;
            userToCreate.PasswordSalt = passwordSalt;

            userRepository.CreateUserAsync(userToCreate);
            return Result<string>.Success("User created successfully");
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
