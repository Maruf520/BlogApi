using AutoMapper;
using Blog.Dtos.Posts;
using Blog.Models;
using Blog.Models.UserModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Repositories.PostRepository
{
    public class PostRepository : IPostRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BlogContext blogContext;

        public PostRepository(IHttpContextAccessor httpContextAccessor, BlogContext blogContext)
        {
            _httpContextAccessor = httpContextAccessor;
            this.blogContext = blogContext;
        }

/*        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));*/

        public async Task<Post> CreateAsync(Post createPostDto)
        {
            await blogContext.Posts.AddAsync(createPostDto);
            await blogContext.SaveChangesAsync();

            return createPostDto;
        }

        public async Task<Post> GetById(int postId, int userId)
        {
            var post =  blogContext.Posts.FirstOrDefault(x => x.Id == postId && x.User.Id == userId);
            return post;
        }

        public  async Task<Post> UpdateAsync (Post updatePost)
        {

             blogContext.Posts.Update(updatePost);
             blogContext.SaveChanges();

            return updatePost;
        }

       
    }
}
