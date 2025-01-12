using Blog.Models.UserModel;
using System;

namespace Blog.Models.Posts
{
    public class Like : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
