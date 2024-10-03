using NanhiDuniya.Services.AuthAPI.Models;
using NanhiDuniya.Services.AuthAPI.Models.Dto;

namespace NanhiDuniya.Services.AuthAPI.Service.IService
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
