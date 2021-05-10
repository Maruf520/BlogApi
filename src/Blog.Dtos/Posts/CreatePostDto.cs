using Blog.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Dtos.Posts
{
    public class CreatePostDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public BloodGroup BloodGrop { get; set; }
        public string Address { get; set; }
        public int Quantity { get; set; }
        public DateTime DateTime { get; set; }

    }
}
