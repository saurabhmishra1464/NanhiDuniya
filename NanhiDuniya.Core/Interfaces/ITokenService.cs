using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Resources.AccountDtos;
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
        Task RevokeRefreshToken(string userId);
        Task<LoginResponse> VerifyRefreshToken(string refreshToken, string userName);
        Task AddRefreshTokenAsync(UserRefreshToken refreshToken);
        Task DeleteRefreshTokenAsync(UserRefreshToken refreshToken);
        bool HasTokenExpired(string token);
    }
}
