using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Blog.Services.Helpers
{
    public class ApplicationUserHelper : IApplicationUserHelper
    {
        private readonly IHttpContextAccessor _httpContext;

        public ApplicationUserHelper(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public string UserId
        {
            get => _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
