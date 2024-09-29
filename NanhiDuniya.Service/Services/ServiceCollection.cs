
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Resources.ResponseDtos;
using NanhiDuniya.Data.Entities;
using NanhiDuniya.Data.Repositories;
using NanhiDuniya.Service.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NanhiDuniya.Service.Services
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection DataServiceCollection(this IServiceCollection services, IConfiguration Configuration)
        {
            // Register AutoMapper
            
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.Configure<NanhiDuniyaServicesSettings>(Configuration.GetSection("NanhiDuniyaServices"));
            services.AddHttpContextAccessor();
            services.AddHttpClient<EmailClientService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<ITokenRepository, TokenRepository>();
            //services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailClientService, EmailClientService>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IImageService, ImageService>();
            return services;
        }
    }
}
