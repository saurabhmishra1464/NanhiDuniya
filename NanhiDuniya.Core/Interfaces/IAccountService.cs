using Microsoft.AspNetCore.Http;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Resources.AccountDtos;
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
        Task<LoginResponse> Login(LoginModel model);
        Task<ResultResponse> Register(RegisterModel model);
        Task<ResultResponse> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<ResultResponse> ValidateResetToken(string resetToken, string Email);
        Task<ResultResponse> PutUserAsync(UserInfoDto userInfo);
        Task<UserInfoDto> GetUser(string userId);
    }
}
