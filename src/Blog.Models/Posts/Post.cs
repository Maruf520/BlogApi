using Blog.Models.UserModel;
using System;
using System.Collections.Generic;

namespace Blog.Models.Posts
{
    public class Post : BaseEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid UserId { get; set; }
        public virtual ApplicationUser Author { get; set; }
        // Navigation properties
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
