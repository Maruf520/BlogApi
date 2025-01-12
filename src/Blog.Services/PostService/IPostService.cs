using Blog.Dtos.Posts;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.PostService
{
    public interface IPostService
    {
        Task<Result<string>> UpdatePostAsync(UpdatePostDto updatePostDto, Guid userId);
        Task<Result<List<PostDto>>> GetAllPosts();
        Task<Result<string>> CreatePostAsync(CreatePostDto createPostDto, Guid userId);
        Task<Result<PostDto>> GetPostById(string id);
        Task<Result<string>> DeletePost(string id);
    }
}
