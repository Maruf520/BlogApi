using System;

namespace Blog.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
