using System;
using System.Collections.Generic;

namespace Blog.Dtos.Roles_and_Claims
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Permissions { get; set; }
    }
}
