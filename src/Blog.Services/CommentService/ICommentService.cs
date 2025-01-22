using Blog.Dtos.Comment;
using Blog.Models;
using Blog.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.CommentService
{
    public interface ICommentService
    {
        Task<Result<string>> DeleteCommentAsync(CreateCommentDto commentDto);
        Task<Result<string>> UpdateCommentAsync(CreateCommentDto commentDto);
        Task<Result<string>> SaveCommentAsync(CreateCommentDto commentDto);
        Task<Result<List<Comment>>> CommentByPostAsync(string postId);
    }
}
