using Blog.Dtos.Users;
using Blog.Models;
using Blog.Services.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthService authService;
        public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            this.authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            ServiceResponse<int> response = await authService.Register(
                new UserDto
                {
                    FirstName = userRegisterDto.FirstName,
                    LastName = userRegisterDto.LastName,
                    BloodGroup = userRegisterDto.BloodGroup,
                    Address = userRegisterDto.Address,
                    Email = userRegisterDto.Email,
                    LastDateOfDonation = userRegisterDto.LastDateOfDonation,
                    Mobile = userRegisterDto.Mobile
                },
                userRegisterDto.Password
                );
            if (!response.Success)
            {
                return BadRequest(response);
            }

            response.Success = true;
            response.Message = "Registered Successfully";

            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            ServiceResponse<string> response = await authService.Login(userLoginDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


    }
}
