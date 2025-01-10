using System;

namespace Blog.Dtos.Roles_and_Claims
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
