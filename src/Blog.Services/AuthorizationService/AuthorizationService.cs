using Blog.Models;
using Blog.Models.UserModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services.AuthorizationService
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthorizationService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> HasPermissionAsync(Guid userId, string permission)
        {
            var permissions = await GetUserPermissionsAsync(userId);

            return permissions.Contains(permission);
        }

        public async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var role = await _context.Roles.FindAsync(roleId.ToString());

            if(user == null || role == null)
            {
                return false;
            }

            var userRole = new UserRole
            {
                RoleId = roleId,
                UserId = userId,
            };

            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
        {
            var role = await _context.Roles.FindAsync(roleId.ToString());
            var permission  = await _context.Permissions.FindAsync(permissionId.ToString());

            if (role == null || permission == null)
                return false;

            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            await _context.RolePermissions.AddAsync(rolePermission);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId)
        {
            return await _context.Users.Where(x => x.Id == userId)
                .SelectMany(x => x.UserRoles)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToListAsync();
        }
    }
}
