using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Dtos.Roles_and_Claims
{
    public class CreateRoleDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public List<string> PermissionIds { get; set; }
    }
}
