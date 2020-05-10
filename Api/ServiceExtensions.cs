using ApplicationCore.Handlers;
using ApplicationCore.Queries;
using DryIoc;
using Dtos;
using ImTools;
using Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Api
{
    public static class ServiceExtensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["AppSettings:Token"])),
                        ValidateAudience = false
                    };
                });
        }
        public static void ConfigureMediatR(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRequestHandler<GetAllQuery<CategoryDto, Category>, IEnumerable<CategoryDto>>), typeof(GetAllQueryHandler<CategoryDto, Category>));
            services.AddTransient(typeof(IRequestHandler<GetByIdQuery<CategoryDto, Category>, CategoryDto>), typeof(GetByIdQueryHandler<CategoryDto, Category>));
            services.AddTransient(typeof(IRequestHandler<GetAllQuery<DiscountDto, Discount>, IEnumerable<DiscountDto>>), typeof(GetAllQueryHandler<DiscountDto, Discount>));
            services.AddTransient(typeof(IRequestHandler<GetByIdQuery<DiscountDto, Discount>, DiscountDto>), typeof(GetByIdQueryHandler<DiscountDto, Discount>));
            services.AddTransient(typeof(IRequestHandler<GetAllQuery<ItemDto, Item>, IEnumerable<ItemDto>>), typeof(GetAllQueryHandler<ItemDto, Item>));
            services.AddTransient(typeof(IRequestHandler<GetByIdQuery<ItemDto, Item>, ItemDto>), typeof(GetByIdQueryHandler<ItemDto, Item>));
            services.AddMediatR(Assembly.Load("ApplicationCore"));
        }
    }
}
