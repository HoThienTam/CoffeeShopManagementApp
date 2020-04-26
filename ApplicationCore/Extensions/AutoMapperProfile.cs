using AutoMapper;
using Dtos;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Item, ItemDto>();
            CreateMap<Item, ItemDto>().ReverseMap();
        }
    }
}
