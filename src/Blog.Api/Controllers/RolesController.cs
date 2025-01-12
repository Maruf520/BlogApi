using Blog.Models.UserModel;
using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blog.Services.AuthorizationService;
using Blog.Api.Attributes;
using Blog.Dtos.Roles_and_Claims;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

    public RolesController(
    RoleManager<Role> roleManager,
    ApplicationDbContext context,
    IAuthorizationService authorizationService)
        {
            _roleManager = roleManager;
            _context = context;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        //[HasPermission("ViewRoles")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            var roles = await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Permissions = r.RolePermissions
                        .Select(rp => rp.Permission.Name)
                        .ToList()
                })
                .ToListAsync();

            return Ok(roles);
        }

        [HttpGet("{id}")]
        //[HasPermission("ViewRoles")]
        public async Task<ActionResult<RoleDto>> GetRole(string id)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id.ToString() == id);

            if (role == null)
                return NotFound();

            var roleDto = new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                Permissions = role.RolePermissions
                    .Select(rp => rp.Permission.Name)
                    .ToList()
            };

            return Ok(roleDto);
        }

        [HttpPost]
        //[HasPermission("CreateRole")]
        public async Task<ActionResult<RoleDto>> CreateRole(CreateRoleDto dto)
        {
            var role = new Role
            {
                Name = dto.Name,
                Description = dto.Description,
                NormalizedName = dto.Name.ToUpper()
            };

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (dto.PermissionIds?.Any() == true)
            {
                var rolePermissions = dto.PermissionIds.Select(permissionId =>
                    new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = Guid.Parse(permissionId)
                    });

                await _context.RolePermissions.AddRangeAsync(rolePermissions);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetRole), new { id = role.Id },
                new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                });
        }
        [HttpPut("{id}")]
        //[HasPermission("UpdateRole")]
        public async Task<IActionResult> UpdateRole(string id, UpdateRoleDto dto)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Id.ToString() == id);

            if (role == null)
                return NotFound();

            if (!string.IsNullOrEmpty(dto.Name))
            {
                role.Name = dto.Name;
                role.NormalizedName = dto.Name.ToUpper();
            }

            if (!string.IsNullOrEmpty(dto.Description))
            {
                role.Description = dto.Description;
            }

            if (dto.PermissionIds != null)
            {
                _context.RolePermissions.RemoveRange(role.RolePermissions);

                var newRolePermissions = dto.PermissionIds.Select(permissionId =>
                    new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = Guid.Parse(permissionId)
                    });

                await _context.RolePermissions.AddRangeAsync(newRolePermissions);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
       //[HasPermission("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
                return NotFound();

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return NoContent();
        }
    }
}
