using Blog.Api.Attributes;
using Blog.Dtos.Roles_and_Claims;
using Blog.Models;
using Blog.Models.UserModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PermissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PermissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [HasPermission("Edit")]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetPermissions()
        {
            var permissions = await _context.Permissions
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                })
                .ToListAsync();

            return Ok(permissions);
        }

        [HttpPost]
        [HasPermission("CreatePermission")]
        public async Task<ActionResult<PermissionDto>> CreatePermission(PermissionDto dto)
        {
            var permission = new Permission
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();

            dto.Id = permission.Id;
            return CreatedAtAction(nameof(GetPermission), new { id = permission.Id }, dto);
        }

        [HttpGet("{id}")]
        [HasPermission("ViewPermissions")]
        public async Task<ActionResult<PermissionDto>> GetPermission(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);

            if (permission == null)
                return NotFound();

            return new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description
            };
        }

        [HttpPut("{id}")]
        [HasPermission("UpdatePermission")]
        public async Task<IActionResult> UpdatePermission(int id, PermissionDto dto)
        {
            var permission = await _context.Permissions.FindAsync(id);

            if (permission == null)
                return NotFound();

            permission.Name = dto.Name;
            permission.Description = dto.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [HasPermission("DeletePermission")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);

            if (permission == null)
                return NotFound();

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
