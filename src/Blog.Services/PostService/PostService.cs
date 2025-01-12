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
        //private readonly IRepository  _repository;
        public PostService(IPostRepository postRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IApplicationUserHelper applicationUserHelper)
        {
            this._postRepository = postRepository;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            this._userRepository = userRepository;
            _applicationUserHelper = applicationUserHelper;
            // _repository = repository;
        }

        public async Task<Result<string>> CreatePostAsync(CreatePostDto createPostDto, Guid userId)
        {
            
            var userToCreate = mapper.Map<Post>(createPostDto);
            userToCreate.CreatedBy = _applicationUserHelper.UserId;
            userToCreate.UserId = Guid.Parse(_applicationUserHelper.UserId);
            userToCreate.CreatedAt = DateTime.Now;
            userToCreate.Id = Guid.NewGuid();

            var createPost = await _postRepository.CreateAsync(userToCreate);

            return Result<string>.Success("Post Created Successfully");

        }

        public async Task<Result<string>> UpdatePostAsync(UpdatePostDto updatePostDto, Guid userId)
        {

            var userid = userId;
            var userPost = await _postRepository.GetById(userid, Guid.Parse(updatePostDto.Id));
            userPost.Body = updatePostDto.Body;
            userPost.Title = updatePostDto.Title;
            var mappedpost = mapper.Map<Post>(userPost);
            var postToUpdate = await _postRepository.UpdateAsync(mappedpost);

            return Result<string>.Success("Post updated Successfully");
        }

        public async Task<Result<List<PostDto>>> GetAllPosts()
        {
            var posts = await _postRepository.GetAllPost();

            var allPOsts = (mapper.Map<List<PostDto>>(posts));

            return Result<List<PostDto>>.Success(allPOsts);
        }
        public async Task<Result<PostDto>> GetPostById(string id)
        {
            var post = await _postRepository.GetPostById(Guid.Parse(id));
            var result = mapper.Map<PostDto>(post);

            return Result<PostDto>.Success(result);
        }
        public async Task<Result<string>> DeletePost(string id)
        {
            var post = await _postRepository.DeletePost(Guid.Parse(id));

            return Result<string>.Success("Post Deleted succesfully");
        }
    }
}
