using Blog.Dtos.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.PostService
{
    public interface IPostService
    {
        Task<PostDto> CreatePostAsync(CreatePostDto createPostDto);
        Task<PostDto> UpdatePostAsync(UpdatePostDto updatePostDto);
    }
}
