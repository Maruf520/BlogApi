using Blog.Dtos.Users;
using Blog.Services.AuthorizationService;
using Blog.Services.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService)
        {
            this.authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
         {
            var response = await authService.Register(userRegisterDto);

            return response.IsSuccess ? Ok(response) : BadRequest(response);

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
