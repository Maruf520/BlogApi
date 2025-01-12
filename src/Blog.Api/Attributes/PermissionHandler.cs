using Microsoft.AspNetCore.Authorization;
using Auth = Blog.Services.AuthorizationService;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace Blog.Api.Attributes
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly Auth.IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionHandler(
            Auth.IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (await _authorizationService.HasPermissionAsync(Guid.Parse(userId), requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
