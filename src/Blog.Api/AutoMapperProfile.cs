using AutoMapper;
using Blog.Dtos.Users;
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
            CreateMap<UserDto, User>();
        }
    }
}
