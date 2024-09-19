using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Resources.AccountDtos;
using NanhiDuniya.Core.Resources.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Service.Services
{
    public interface ITokenService
    {
        Task<String> GenerateRefreshToken();
        Task<string> GenerateAccessToken(string email, string userId);
        Task<ApiResponse<object>> RevokeRefreshToken(string userId);
        Task<ApiResponse<object>> VerifyRefreshToken();
        Task AddRefreshTokenAsync(UserRefreshToken refreshToken);
        Task DeleteRefreshTokenAsync(UserRefreshToken refreshToken);
        bool HasTokenExpired(string token);
        Task<string> GenerateConfirmEmailToken(string email);
        Task<string> GenerateResetPasswordToken(string email);
    }
}
