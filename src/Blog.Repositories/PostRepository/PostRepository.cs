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

/*        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));*/

        public async Task<PostDto> CreateAsync(CreatePostDto createPostDto)
        {
            var userid = 1;
            var userToCreate = mapper.Map<Post>(createPostDto);
            userToCreate.CreatedAt = DateTime.Now;
            userToCreate.User = blogContext.Users.FirstOrDefault(x => x.Id == userid);
            
            await blogContext.Posts.AddAsync(userToCreate);
            await blogContext.SaveChangesAsync();

            return mapper.Map<PostDto>(userToCreate);
        }

        public  async Task<PostDto> UpdateAsync (UpdatePostDto updatePostDto)
        {
            var userid = 1;
            var userPost =  blogContext.Posts.FirstOrDefault(x => x.User.Id == userid && x.Id == updatePostDto.Id);
            userPost.Body = updatePostDto.Body;
            userPost.Title = updatePostDto.Title;
            userPost.UpdatedAt = DateTime.Now;
            userPost.User = blogContext.Users.FirstOrDefault(x => x.Id == userid);
            userPost.Address = updatePostDto.Address;
            userPost.NeededAt = updatePostDto.NeededAt;
            userPost.BloodGrop = updatePostDto.BloodGrop;
            userPost.Quantity = updatePostDto.Quantity;

            blogContext.Posts.Update(userPost);
            blogContext.SaveChanges();

            return mapper.Map<PostDto>(userPost);
        }

       
    }
}
