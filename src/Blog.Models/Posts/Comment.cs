using Blog.Models.UserModel;
using System;

namespace Blog.Models.Posts
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}
