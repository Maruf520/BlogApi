using Blog.Dtos.Posts;
using Blog.Services.PostService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostService postService;
        public PostController(IHttpContextAccessor httpContextAccessor, IPostService postService)
        {
            _httpContextAccessor = httpContextAccessor;
            this.postService = postService;
        }

        [HttpGet("get-posts")]
        public IActionResult GetPosts()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(new { Message = "Authentication works!", UserId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostDto createPostDto)
        {
            var x = await postService.CreatePostAsync(createPostDto);
            return Ok(x);
        
        }

        [HttpPut("update")]

        public async Task<IActionResult> UpdatePost (UpdatePostDto updatePostDto)
        {
            var updatedPost = await postService.UpdatePostAsync(updatePostDto);
            return Ok(updatedPost);
        }
        [HttpGet("getallposts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await postService.GetAllPosts();
            return posts.IsSuccess ? Ok(posts) : BadRequest(posts.Error.Message);
        }

        [HttpGet("getpostbyid")]
        public async Task<IActionResult> GetPostById(string Id)
        {
            var posts = await postService.GetPostById(Id);
            return posts.IsSuccess ? Ok(posts) : BadRequest(posts.Error.Message);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleetPost(string id)
        {
            var post = await postService.DeletePost(id);
            return post.IsSuccess ? Ok(post) : BadRequest(post.Error.Message);
        }

        private Guid GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null || !Guid.TryParse(userId, out var parsedUserId))
            {
                throw new InvalidOperationException("User ID is not available or is invalid.");
            }

            return parsedUserId;
        }
    }
}
