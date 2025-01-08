using Blog.Dtos.Users;
using Blog.Services.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var response = await authService.Register(userRegisterDto);

            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var response = await authService.Login(userLoginDto);
            return response.IsSuccess ? Ok(response.Data) : BadRequest(response);
        }
    }
}
