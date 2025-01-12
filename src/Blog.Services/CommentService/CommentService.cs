using AutoMapper;
using Blog.Dtos.Comment;
using Blog.Models;
using Blog.Models.Posts;
using Blog.Repositories;
using Blog.Services.Helpers;
using System;
using System.Threading.Tasks;

namespace Blog.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly IRepository _repository;
        private IApplicationUserHelper _applicationUserHelper;
        private readonly IMapper _mapper;
        public CommentService(IApplicationUserHelper applicationUserHelper, IRepository repository, IMapper mapper)
        {
            _applicationUserHelper = applicationUserHelper;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<string>> SaveCommentAsync(CreateCommentDto commentDto)
        {
            var post = await _repository.FirstOrDefaultAsync<Post>(X => X.Id.ToString() == commentDto.PostId);

            if (post == null)
            {
                return Result<string>.Failure("No Post Found");
            }
            var userId = _applicationUserHelper.UserId;
            var model = _mapper.Map<Comment>(commentDto);
            model.CreatedBy = userId;
            model.CreatedAt = DateTime.UtcNow;

            await _repository.AddAsync(model);

            return Result<string>.Success("No Post Found");
        }

        public async Task<Result<string>> UpdateCommentAsync(CreateCommentDto commentDto)
        {
            var post = await _repository.FirstOrDefaultAsync<Post>(X => X.Id.ToString() == commentDto.PostId);

            if (post == null)
            {
                return Result<string>.Failure("No Post Found");
            }
            var comment = await _repository.FirstOrDefaultAsync<Comment>(x => x.Id.ToString() == commentDto.CommentId && x.PostId.ToString() == commentDto.PostId);
            if (post == null)
            {
                return Result<string>.Failure("No Comment Found");
            }
            comment.Content = commentDto.Content;
            comment.LastModified = DateTime.UtcNow;
            comment.LastModifiedBy = _applicationUserHelper.UserId;

            await _repository.UpdateAsync(comment);

            return Result<string>.Success("Comment Updated");
        }

        public async Task<Result<string>> DeleteCommentAsync(CreateCommentDto commentDto)
        {
            var post = await _repository.FirstOrDefaultAsync<Post>(X => X.Id.ToString() == commentDto.PostId);

            if (post == null)
            {
                return Result<string>.Failure("No Post Found");
            }
            var comment = await _repository.FirstOrDefaultAsync<Comment>(x => x.Id.ToString() == commentDto.CommentId && x.PostId.ToString() == commentDto.PostId);
            if (post == null)
            {
                return Result<string>.Failure("No Comment Found");
            }

            await _repository.DeleteAsync(comment);

            return Result<string>.Success("Comment Deleted");
        }
    }
}
