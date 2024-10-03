using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NanhiDuniya.Services.AuthAPI.Data.IRepositories;
using NanhiDuniya.Services.AuthAPI.Data.Repositories;
using NanhiDuniya.Services.AuthAPI.Service.IService;
using NanhiDuniya.Services.AuthAPI.Service.Mapping;
using static NanhiDuniya.Services.AuthAPI.Settings.NanhiDuniyaServices;

namespace NanhiDuniya.Services.AuthAPI.Service
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection DataServiceCollection(this IServiceCollection services, IConfiguration Configuration)
        {
            // Register AutoMapper

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.Configure<NanhiDuniyaServicesSettings>(Configuration.GetSection("NanhiDuniyaServices"));
            services.AddHttpContextAccessor();
            //services.AddHttpClient<EmailClientService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            //services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            //services.AddScoped<IEmailClientService, EmailClientService>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IImageService, ImageService>();
            return services;
        }
    }
}
