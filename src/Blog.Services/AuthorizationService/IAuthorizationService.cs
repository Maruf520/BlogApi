using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Services.AuthorizationService
{
    public interface IAuthorizationService
    {
        Task<bool> HasPermissionAsync(Guid userId, string permission);
        Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId);
        Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId);
        Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId);
    }
}
