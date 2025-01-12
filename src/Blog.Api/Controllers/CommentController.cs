using Blog.Dtos.Comment;
using Blog.Services.CommentService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCommentAsync(CreateCommentDto commentDto)
        {
            var comment = await _commentService.SaveCommentAsync(commentDto);
            return Ok(comment);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCommentAsync(CreateCommentDto commentDto)
        {
            var comment = await _commentService.UpdateCommentAsync(commentDto);
            return Ok(comment);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCommentAsync(CreateCommentDto commentDto)
        {
            var comment = await _commentService.DeleteCommentAsync(commentDto);
            return Ok(comment);
        }
    }
}
