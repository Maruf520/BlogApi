using Blog.Dtos.Users;
using System;

namespace Blog.Dtos.Posts
{
    public class PostDto
    {

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public GetUserDto User { get; set; }
    }
}
