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
        Task<string> GenerateAccessToken(string userId);
        Task RevokeRefreshToken(string userId);
        Task<RefreshTokenResponse> VerifyRefreshToken(RefreshTokenDto request);
        Task AddRefreshTokenAsync(UserRefreshToken refreshToken);
        Task DeleteRefreshTokenAsync(UserRefreshToken refreshToken);
    }
}
