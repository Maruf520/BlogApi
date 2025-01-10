using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Blog.Models.UserModel
{
    public class Role : IdentityRole<Guid>
    {
        public string Description { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
