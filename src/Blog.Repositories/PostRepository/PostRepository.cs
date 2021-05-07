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
        private readonly IMapper mapper;

        public PostRepository(IHttpContextAccessor httpContextAccessor, IMapper mapper, BlogContext blogContext)
        {
            _httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.blogContext = blogContext;
        }

        public async Task<PostDto> CreateAsync(CreatePostDto createPostDto)
        {
            var userid = 1;
            
            Post post = new Post
            {
                Body = createPostDto.Body,
                Title = createPostDto.Title,
                CreatedAt = DateTime.Now
                
            };
            post.User =  blogContext.Users.FirstOrDefault(x => x.Id == userid);
            
            await blogContext.Posts.AddAsync(post);
            await blogContext.SaveChangesAsync();

            var user = blogContext.Posts.Where(x => x.UserId == userid).ToList();

            return mapper.Map<PostDto>(post);
        }

       
    }
}
