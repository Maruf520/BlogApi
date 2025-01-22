using AutoMapper;
using Blog.Dtos.Posts;
using Blog.Models;
using Blog.Models.Posts;
using Blog.Repositories;
using Blog.Repositories.PostRepository;
using Blog.Repositories.Users;
using Blog.Services.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Services.PostService
{
    [Authorize]
    public class PostService : IPostService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationUserHelper _applicationUserHelper;
        private readonly IRepository _repository;
        public PostService(IPostRepository postRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IApplicationUserHelper applicationUserHelper, IRepository repository)
        {
            this._postRepository = postRepository;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            this._userRepository = userRepository;
            _applicationUserHelper = applicationUserHelper;
            _repository = repository;
        }

        public async Task<Result<string>> CreatePostAsync(CreatePostDto createPostDto)
        {
            var userId = _applicationUserHelper.UserId;
            var post = mapper.Map<Post>(createPostDto);
            post.CreatedBy = userId;
            post.UserId = Guid.Parse(userId);
            post.CreatedAt = DateTime.Now;
            post.Id = Guid.NewGuid();

            var createPost = await _postRepository.CreateAsync(post);

            return Result<string>.Success("Post Created Successfully");
        }

        public async Task<Result<string>> UpdatePostAsync(UpdatePostDto updatePostDto)
        {
            var post = await _repository.FirstOrDefaultAsync<Post>(x => x.Id.ToString() == updatePostDto.Id);
            if (post == null)
            {
                return Result<string>.Failure("Post not found");
            }
            post.Body = updatePostDto.Body;
            post.Title = updatePostDto.Title;
            var model = mapper.Map<Post>(post);
            await _repository.UpdateAsync(post);

            return Result<string>.Success("Post updated Successfully");
        }

        public async Task<Result<List<PostDto>>> GetAllPosts()
        {
            var posts = await _repository.Query<Post>(includes: c => c.Include(p => p.Comments).Include(x => x.Author)).ToListAsync();
            //if (posts.Count == 0)
            //{
            //    return Result<List<PostDto>>.Failure("No Post Found");

            //}
            var allPOsts = (mapper.Map<List<PostDto>>(posts));

            return Result<List<PostDto>>.Success(allPOsts);
        }

        public async Task<Result<PostDto>> GetPostById(string id)
        {

            var post = await _repository.Query<Post>(includes: x => x.Include(x => x.Comments).Include(x => x.Author)).FirstOrDefaultAsync();
            if (post == null)
            {
                return Result<PostDto>.Failure("Post Not found");
            }
            var result = mapper.Map<PostDto>(post);

            return Result<PostDto>.Success(result);
        }
        public async Task<Result<string>> DeletePost(string id)
        {
            var post = await _repository.FirstOrDefaultAsync<Post>(x => x.Id.ToString() == id);
            if (post == null)
            {
                return Result<string>.Failure("Post Not found");
            }
            await _repository.DeleteAsync(post);

            return Result<string>.Success("Post Deleted succesfully");
        }
    }
}
