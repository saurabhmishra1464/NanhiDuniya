using Microsoft.Extensions.Configuration;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Resources.AccountDtos;
using NanhiDuniya.Data.Entities;
using System.Web;

namespace NanhiDuniya.Service.Services
{
    public class UserService:IUserService
    {
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateResetPasswordLink(UserDto user, string token)
        {
            var frontendUrl = _configuration["BaseUrls:Frontend"];
            var encodedToken = HttpUtility.UrlEncode(token);
            var encodedEmail = HttpUtility.UrlEncode(user.Email);

            return $"{frontendUrl}/auth/resetpassword?token={encodedToken}&email={encodedEmail}";
        }

        public string GenerateVerifyEmailLink(UserDto user, string token)
        {
            var frontendUrl = _configuration["BaseUrls:Frontend"];
            var encodedToken = HttpUtility.UrlEncode(token);
            var encodedEmail = HttpUtility.UrlEncode(user.Email);

            return $"{frontendUrl}/auth/verifyEmail?token={encodedToken}&email={encodedEmail}";
        }
    }

}
