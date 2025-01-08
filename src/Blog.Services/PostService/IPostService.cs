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
        Task<Result<string>> UpdatePostAsync(UpdatePostDto updatePostDto, int userId);
        Task<Result<List<PostDto>>> GetAllPosts();
        Task<Result<string>> CreatePostAsync(CreatePostDto createPostDto, int userId);
        Task<Result<PostDto>> GetPostById(int id);
        Task<Result<string>> DeletePost(int id);
    }
}
