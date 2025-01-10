using Microsoft.AspNetCore.Identity;
using System;

namespace Blog.Models.UserModel
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual Role Role { get; set; }
    }
}
