using Blog.Dtos.Posts;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repositories.PostRepository
{
    public interface IPostRepository
    {
        Task<Post> CreateAsync(Post createPostDto);
        Task<Post> UpdateAsync(Post updatePost);
        Task<Post> GetById(int postId, int userId);
    }
}
