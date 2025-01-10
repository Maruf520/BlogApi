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
        Task<Post> GetById(Guid userId, int postId);
        Task<List<Post>> GetAllPost();
        Task<Post> GetPostById(int id);
        Task<Post> DeletePost(int id);
    }
}
