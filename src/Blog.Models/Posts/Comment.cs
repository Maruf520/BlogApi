using Blog.Models.UserModel;
using System;

namespace Blog.Models.Posts
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}
