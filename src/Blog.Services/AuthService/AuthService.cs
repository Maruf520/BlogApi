using AutoMapper;
using Blog.Dtos.Users;
using Blog.Models;
using Blog.Models.UserModel;
using Blog.Repositories.Users;
using Blog.Services.UserExtentionService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IUserExtentionService userExtentionService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(IUserRepository userRepository, IUserExtentionService userExtentionService, IMapper mapper, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.userExtentionService = userExtentionService;
            this.mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        public async Task<Result<LoginResponse>> Login(UserLoginDto userDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);

                if (user == null)
                {
                    return Result<LoginResponse>.Failure("Invalid login attempt. User not found.");
                }
                var result = await _userManager.CheckPasswordAsync(user, userDto.Password);
                if (result != false)
                {
                    var userRoles = await _context.UserRoles
                     .Include(ur => ur.Role)
                     .Where(ur => ur.UserId == user.Id)
                     .Select(ur => ur.Role.Name)
                     .ToListAsync();

                    var permissions = await _context.UserRoles
                        .Where(ur => ur.UserId == user.Id)
                        .SelectMany(ur => ur.Role.RolePermissions)
                        .Select(rp => rp.Permission.Name)
                        .Distinct()
                        .ToListAsync();
                    var token =  await GenerateJwtToken(user, userRoles, permissions);
                    //var user = userRepository.GetByEmail(userDto.Email);
                    var response = new LoginResponse();

                    response.Token = token.Token;
                    response.ExpiresInSeconds = 7000;
                    response.User.FirstName = user.FirstName;
                    response.User.FirstName = user.FirstName;
                    response.User.LastName = user.LastName;
                    response.User.Mobile = user.Mobile;
                    response.User.Address = user.Address;
                    response.User.Image = user.Image;
                    response.User.Email = user.Email;

                    return Result<LoginResponse>.Success(response);
                }
                return Result<LoginResponse>.Failure("Invalid login attempt. ");
            }
            catch(Exception ex)
            {
                throw new Exception();
            }

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


           await userRepository.CreateUserAsync(user);
            return Result<string>.Success("User created successfully");
        }
        private async Task<LoginResponse> GenerateJwtToken(ApplicationUser user, List<string> roles, List<string> permissions)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("FirstName", user.FirstName ?? ""),
            new Claim("LastName", user.LastName ?? "")
        };


            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permission", permission));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(24); 

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresInSeconds = (int)expiration.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }

    }
}
