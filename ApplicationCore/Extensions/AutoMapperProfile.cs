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
            CreateMap<Item, ItemDto>().ReverseMap();
            CreateMap<Item, ItemForInvoiceDto>().AfterMap((src, dest) => dest.IsAdded = true).ReverseMap();
            CreateMap<Invoice, InvoiceDto>().ReverseMap();
            CreateMap<Invoice, InvoiceForCreateDto>().ReverseMap();
            CreateMap<Discount, DiscountDto>().ReverseMap();
            CreateMap<Discount, DiscountForCreateDto>().ReverseMap();
            CreateMap<Discount, DiscountForInvoiceDto>().AfterMap((src, dest) => dest.IsAdded = true).ReverseMap();
            CreateMap<Table, TableDto>().ReverseMap();
            CreateMap<Zone, ZoneDto>().ReverseMap();
            CreateMap<Session, SessionDto>().ReverseMap();
            CreateMap<ImportExportHistory, ImportExportHistoryDto>().ReverseMap();
        }
    }
}
