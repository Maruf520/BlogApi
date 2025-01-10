using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Blog.Models.UserModel
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }

    }
}
