using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Resources.ResponseDtos;
using NanhiDuniya.Data.Entities;
using NanhiDuniya.Service.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            services.AddHttpClient<EmailClientService>();
            services.AddScoped<IAccountService, AccountService>();
            //services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailClientService, EmailClientService>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddScoped<IUserService, UserService>();
            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                {
                    var res = new ResponseDTO
                    {
                        Message = "One or more validation errors occurred.",
                        StatusCode = HttpStatusCode.BadRequest,
                        // Add additional properties or data from ModelState if needed
                        Errors = actionContext.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToArray()
                    };
                    return new ObjectResult(res)
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                    };
                };
            });
            return services;
        }
    }
}
