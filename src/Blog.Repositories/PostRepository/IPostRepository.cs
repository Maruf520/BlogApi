using Blog.Models.Posts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repositories.PostRepository
{
    public interface IPostRepository
    {
        Task<Post> CreateAsync(Post createPostDto);
        Task<Post> UpdateAsync(Post updatePost);
        Task<Post> GetById(Guid userId, Guid postId);
        Task<List<Post>> GetAllPost();
        Task<Post> GetPostById(Guid id);
        Task<Post> DeletePost(Guid id);
    }
}
