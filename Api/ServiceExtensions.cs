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
            services.AddMediatR(Assembly.Load("ApplicationCore"));
        }
    }
}
