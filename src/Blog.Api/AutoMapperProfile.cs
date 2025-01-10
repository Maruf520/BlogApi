using AutoMapper;
using Blog.Dtos.Posts;
using Blog.Dtos.Users;
using Blog.Models;
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
            CreateMap<Post, PostDto>();
            CreateMap<CreatePostDto, PostDto>();
            CreateMap<UpdatePostDto, PostDto>();
            CreateMap<ApplicationUser, GetUserDto>();
            CreateMap<CreatePostDto, Post>();
            CreateMap<UserRegisterDto, UserDto>();
        }
    }
}
