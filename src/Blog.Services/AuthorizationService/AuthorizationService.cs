using Blog.Models;
using Blog.Models.UserModel;
using Blog.Repositories.Users;
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
        private readonly RoleManager<Role> _roleManager;
        private readonly IUserRepository _userRepository;

        public AuthorizationService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<Role> roleManage,
            IUserRepository userRepository)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManage;
            _userRepository = userRepository;
        }

        public async Task<bool> HasPermissionAsync(Guid userId, string permission)
        {
            var permissions = await GetUserPermissionsAsync(userId);

            return permissions.Contains(permission);
        }

        public async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            try
            {
                var user = await _userRepository.GetById(userId);
                var rol = roleId.ToString();
                var role = await _context.Roles.FindAsync(roleId);

                if (user == null || role == null)
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
            catch (Exception ex) 
            {
                throw new Exception();
            }
        }

        public async Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
                throw new ArgumentException("Role not found");

            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission == null)
                throw new ArgumentException("Permission not found");

            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId,
                Role = role,
                Permission = permission
            };

     
            var existingRolePermission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (existingRolePermission != null)
                return true; 

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
