using AutoMapper;
using Blog.Dtos.Posts;
using Blog.Models;
using Blog.Models.Posts;
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
        private readonly ApplicationDbContext _blogContext;

        public PostRepository(IHttpContextAccessor httpContextAccessor, ApplicationDbContext blogContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _blogContext = blogContext;
        }

       private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<Post> CreateAsync(Post createPostDto)
        {
            try
            {

                await _blogContext.Posts.AddAsync(createPostDto);
                await _blogContext.SaveChangesAsync();

                return createPostDto;
            }
            catch(Exception ex)
            {
                throw new Exception();
            }

        }

        public async Task<Post> GetById(Guid userId, Guid postId)
        {
            var post =  _blogContext.Posts.FirstOrDefault(x => x.Id == postId && x.UserId == userId);
            return post;
        }

        public  async Task<Post> UpdateAsync (Post updatePost)
        {

             _blogContext.Posts.Update(updatePost);
             _blogContext.SaveChanges();

            return updatePost;
        }

        public async Task<List<Post>> GetAllPost()
        {
          var blogs =  await _blogContext.Posts.ToListAsync();
          return blogs;
        }

        public async Task<Post> GetPostById(Guid id)
        {
            var post = await _blogContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
            return post;
        }

        public async Task<Post> DeletePost(Guid id)
        {
            var post = await _blogContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
            _blogContext.Posts.Remove(post);
            await _blogContext.SaveChangesAsync();
            return post;
        }

    }
}
