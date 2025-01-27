using Blog.Dtos.Account;
using Blog.Dtos.Users;
using Blog.Models;
using Blog.Services.AuthService;
using Blog.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        public AccountController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }
        [HttpGet]
        public async Task<IActionResult> GetUser([FromQuery]string email)
        {
            var user = await _userService.GetUser(email);
            return user.IsSuccess ? Ok(user.Data) : BadRequest(user.Error.Message);
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody]PasswordResetRequestDto dto)
        {
            var user = await _userService.GetUser(dto.Email);
            if (user.Data is null)
            {
                return BadRequest("User Not Found. Please contact with admin.");
            }

            var token = await _authService.GeneratePasswordToken(dto.Email);
            var tokenDto = new LoginResponse();
            tokenDto.Token = token.Data;

            return token.IsSuccess ? Ok(tokenDto) : BadRequest(token.Error.Message);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto resetDto)
        {
            var verify = await _authService.ResetPassword(resetDto.Email, resetDto.Token, resetDto.NewPassword);
            
            if(!verify)
            {
                return BadRequest("Invalid Token");
            }
            return Ok("Password Has been reset successfully.");
        }
        private static string GetRequestScheme(bool isHttps)
        {
            return isHttps ? "https" : "http";
        }
    }
}
