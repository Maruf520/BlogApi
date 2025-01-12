using AutoMapper;
using Blog.Dtos.Comment;
using Blog.Dtos.Posts;
using Blog.Dtos.Users;
using Blog.Models.Posts;
using Blog.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDto, ApplicationUser>();
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<CreatePostDto, PostDto>();
            CreateMap<UpdatePostDto, PostDto>();
            CreateMap<ApplicationUser, GetUserDto>();
            CreateMap<CreatePostDto, Post>();
            CreateMap<UserRegisterDto, UserDto>();
            CreateMap<UserRegisterDto, ApplicationUser>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, CreateCommentDto>().ReverseMap();
        }
    }
}
