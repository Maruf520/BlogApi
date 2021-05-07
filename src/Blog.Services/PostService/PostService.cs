using AutoMapper;
using Blog.Dtos.Posts;
using Blog.Repositories.PostRepository;
using Blog.Repositories.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostService(IPostRepository postRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.postRepository = postRepository;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PostDto> CreatePostAsync(CreatePostDto createPostDto)
        {
            var createPost = await postRepository.CreateAsync(createPostDto);

            return mapper.Map<PostDto>(createPost);
            
        }
    }
}
