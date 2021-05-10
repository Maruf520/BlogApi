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
        Task<PostDto> CreateAsync(CreatePostDto createPostDto);
        Task<PostDto> UpdateAsync(UpdatePostDto updatePostDto);
    }
}
