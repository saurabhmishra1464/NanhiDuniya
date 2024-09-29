using Microsoft.AspNetCore.Http;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Resources.AccountDtos;
using NanhiDuniya.Core.Resources.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse<UserProfile>> Login(LoginModel model);
        Task<ApiResponse<object>> Register(RegisterModel model);
        Task<ApiResponse<object>> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<bool> ValidResetToken(string resetToken, string Email);
        Task<ApiResponse<UserProfile>> PutUserAsync(UserInfoDto userInfo);
        Task<ApiResponse<UserProfile>> GetUser();
        Task<ApiResponse<object>> ConfirmEmail(string token, string email);
        Task<ApiResponse<object>> ForgotPassword(string email);
        Task<ApiResponse<object>> SendConfirmationEmail(string email);
    }
}
