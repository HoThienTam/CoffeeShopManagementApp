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
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryForCreateDto>().ReverseMap();
            CreateMap<Item, ItemDto>().ReverseMap();
            CreateMap<Item, ItemForCreateDto>().ReverseMap();
            CreateMap<Item, ItemForInvoiceDto>().ReverseMap();
            CreateMap<Invoice, InvoiceDto>().ReverseMap();
            CreateMap<Invoice, InvoiceForCreateDto>().ReverseMap();
            CreateMap<Discount, DiscountDto>().ReverseMap();
            CreateMap<Discount, DiscountForCreateDto>().ReverseMap();
        }
    }
}
