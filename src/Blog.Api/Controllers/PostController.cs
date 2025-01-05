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
            var x = await postService.CreatePostAsync(createPostDto);
            return Ok(x);
        
        }

        [HttpPut("update")]

        public async Task<IActionResult> UpdatePost (UpdatePostDto updatePostDto)
        {
            var updatedPost = await postService.UpdatePostAsync(updatePostDto);
            return Ok(updatedPost);
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

    }
}
