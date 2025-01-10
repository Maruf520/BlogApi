using AutoMapper;
using Blog.Dtos.Posts;
using Blog.Models;
using Blog.Models.UserModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Repositories.PostRepository
{
    public class PostRepository : IPostRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext blogContext;

        public PostRepository(IHttpContextAccessor httpContextAccessor, ApplicationDbContext blogContext)
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

        public async Task<Post> GetById(Guid userId, int postId)
        {
            var post =  blogContext.Posts.FirstOrDefault(x => x.Id == postId && x.UserId == userId);
            return post;
        }

        public  async Task<Post> UpdateAsync (Post updatePost)
        {

             blogContext.Posts.Update(updatePost);
             blogContext.SaveChanges();

            return updatePost;
        }

        public async Task<List<Post>> GetAllPost()
        {
          var blogs =  await blogContext.Posts.ToListAsync();
          return blogs;
        }

        public async Task<Post> GetPostById(int id)
        {
            var post = await blogContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
            return post;
        }

        public async Task<Post> DeletePost(int id)
        {
            var post = await blogContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
            blogContext.Posts.Remove(post);
            await blogContext.SaveChangesAsync();
            return post;
        }

    }
}
