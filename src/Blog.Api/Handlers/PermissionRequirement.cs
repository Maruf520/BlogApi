using Microsoft.AspNetCore.Authorization;

namespace Blog.Api.Handlers
{
    public class PermissionRequirement: IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
