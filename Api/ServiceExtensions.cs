using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            List<Assembly> assemblies = new List<Assembly>();
            Assembly mscorlib = Assembly.Load("ApplicationCore");
            foreach (Type type in mscorlib.GetTypes().Where(a => a.FullName.EndsWith("Handler")).ToList())
            {
                assemblies.Add(type.Assembly);
            }
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMediatR(assemblies.ToArray());
        }
    }
}
