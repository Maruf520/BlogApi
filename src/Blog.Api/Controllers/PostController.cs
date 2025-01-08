using Blog.Dtos.Posts;
using Blog.Services.PostService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostService postService;
        public PostController(IHttpContextAccessor httpContextAccessor, IPostService postService)
        {
            _httpContextAccessor = httpContextAccessor;
            this.postService = postService;
        }
       

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostDto createPostDto)
        {
            var x = await postService.CreatePostAsync(createPostDto, GetUserId());
            return Ok(x);
        
        }

        [HttpPut("update")]

        public async Task<IActionResult> UpdatePost (UpdatePostDto updatePostDto)
        {
            var updatedPost = await postService.UpdatePostAsync(updatePostDto, GetUserId());
            return Ok(updatedPost);
        }
        [HttpGet("getallposts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await postService.GetAllPosts();
            return posts.IsSuccess ? Ok(posts) : BadRequest(posts.Error.Message);
        }

        [HttpGet("getpostbyid")]
        public async Task<IActionResult> GetPostById(int Id)
        {
            var posts = await postService.GetPostById(Id);
            return posts.IsSuccess ? Ok(posts) : BadRequest(posts.Error.Message);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleetPost(int id)
        {
            var post = await postService.DeletePost(id);
            return post.IsSuccess ? Ok(post) : BadRequest(post.Error.Message);
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

    }
}
