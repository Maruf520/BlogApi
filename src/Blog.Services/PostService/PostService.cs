using AutoMapper;
using Blog.Dtos.Posts;
using Blog.Models;
using Blog.Repositories.PostRepository;
using Blog.Repositories.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public PostService(IPostRepository postRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this._postRepository = postRepository;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            this._userRepository = userRepository;
        }

        public async Task<Result<string>> CreatePostAsync(CreatePostDto createPostDto, Guid userId)
        {
            var userid = userId;
            var userToCreate = mapper.Map<Post>(createPostDto);
            userToCreate.CreatedAt = DateTime.Now;
            userToCreate.UserId = _userRepository.GetById(userid).Id;

            var createPost = await _postRepository.CreateAsync(userToCreate);

            return Result<string>.Success("Post Created Successfully");

        }

        public async Task<Result<string>> UpdatePostAsync(UpdatePostDto updatePostDto, Guid userId)
        {

            var userid = userId;
            var userPost = await _postRepository.GetById(userid, updatePostDto.Id);
            userPost.Body = updatePostDto.Body;
            userPost.Title = updatePostDto.Title;
            userPost.UpdatedAt = DateTime.Now;
            userPost.UserId = _userRepository.GetById(userid).Id;
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
        public async Task<Result<PostDto>> GetPostById(int id)
        {
            var post = await _postRepository.GetPostById(id);
            var result = mapper.Map<PostDto>(post);

            return Result<PostDto>.Success(result);
        }
        public async Task<Result<string>> DeletePost(int id)
        {
            var post = await _postRepository.DeletePost(id);

            return Result<string>.Success("Post Deleted succesfully");
        }
    }
}
