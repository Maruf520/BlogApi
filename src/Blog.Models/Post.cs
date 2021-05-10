using Blog.Models.Enums;
using Blog.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public BloodGroup BloodGrop { get; set; }
        public string Address { get; set; }
        public int Quantity { get; set; }
        public DateTime NeededAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ? UpdatedAt { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
