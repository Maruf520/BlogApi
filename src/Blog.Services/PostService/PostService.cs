using AutoMapper;
using Blog.Dtos.Posts;
using Blog.Models;
using Blog.Repositories.PostRepository;
using Blog.Repositories.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.PostService
{
    [Authorize]
    public class PostService : IPostService
    {
        private readonly IUserRepository userRepository;
        private readonly IPostRepository postRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostService(IPostRepository postRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this.postRepository = postRepository;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
        }

        public async Task<PostDto> CreatePostAsync(CreatePostDto createPostDto)
        {
            var userid = 1;
            var userToCreate = mapper.Map<Post>(createPostDto);
            userToCreate.CreatedAt = DateTime.Now;
            userToCreate.User = userRepository.GetById(userid);

            var createPost = await postRepository.CreateAsync(userToCreate);

            return mapper.Map<PostDto>(createPost);
            
        }

        public async Task<PostDto> UpdatePostAsync (UpdatePostDto updatePostDto)
        {

            var userid = 1;
            var userPost = await postRepository.GetById(userid, updatePostDto.Id);
            userPost.Body = updatePostDto.Body;
            userPost.Title = updatePostDto.Title;
            userPost.UpdatedAt = DateTime.Now;
            userPost.User = userRepository.GetById(userid);
            userPost.Address = updatePostDto.Address;
            userPost.NeededAt = updatePostDto.NeededAt;
            userPost.BloodGrop = updatePostDto.BloodGrop;
            userPost.Quantity = updatePostDto.Quantity;
            var mappedpost = mapper.Map<Post>(userPost);
            var postToUpdate = await postRepository.UpdateAsync(mappedpost);

            return (mapper.Map<PostDto>(postToUpdate));
        }
    }
}
