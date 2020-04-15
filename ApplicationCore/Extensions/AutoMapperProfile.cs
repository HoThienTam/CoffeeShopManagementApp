using AutoMapper;
using Dtos;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
