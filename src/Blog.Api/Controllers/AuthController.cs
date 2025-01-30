using Blog.Api.Content;
using Blog.Dtos.Email;
using Blog.Dtos.Users;
using Blog.Services.AuthorizationService;
using Blog.Services.AuthService;
using Blog.Services.EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly IEmailService _emailService;
        public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService, IEmailService emailService)
        {
            this.authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
            _emailService = emailService;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
         {
            var user = await authService.Register(userRegisterDto);
            if(user.Data == null)
            {
                return user.IsSuccess ? Ok("Verification Email has been sent to your email.") : BadRequest(user.Error.Message);
            }
            var emailConfirmationToken = await authService.GenerateEmailConfirmationTokenAsync(user.Data);

            var emailBody = EmailTemplates.GenerateVerificationEmail($"{user.Data.FirstName + " " + user.Data.LastName}", emailConfirmationToken, 24);
            var emailDto = new EmailDto();
            emailDto.Subject = "Verify Email";
            emailDto.Body = emailBody;
            emailDto.To = user.Data.Email;
            emailDto.IsHtml = true;
           var result =  await _emailService.SendEmailAsync(emailDto);

            return result.IsSuccess ? Ok(result) : BadRequest(result.Error.Message);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var response = await authService.GetUser(userId);

            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }
        [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("get-user")]
        public async Task<IActionResult> GetLoggedInUser()
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await authService.GetUser(userId);

            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }

        [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto dto)
        {
            var user = await authService.UpdateUser(dto);

            return user.IsSuccess ? Ok(user) : BadRequest(user.Error.Message);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var response = await authService.Login(userLoginDto);
            return response.IsSuccess ? Ok(response.Data) : BadRequest(response);
        }

        [HttpPost("add-role-to-user")]
        public async Task<IActionResult> AddRoleToUser(string userId, string roleId)
        {
            var authRole = await _authorizationService.AssignRoleToUserAsync(Guid.Parse(userId), Guid.Parse(roleId));
            return Ok(authRole);
        }

        [HttpPost("add-permission-to-role")]
        public async Task<IActionResult> AddPermissionsToRole(string roleId, string permissionId)
        {
            var permission = await _authorizationService.AssignPermissionToRoleAsync(Guid.Parse(roleId), Guid.Parse(permissionId));

            return Ok(permission);
        }
    }
}
