using NanhiDuniya.Services.AuthAPI.Models;

namespace NanhiDuniya.Services.AuthAPI.Data.IRepositories
{
    public interface ITokenRepository
    {
        Task<UserRefreshToken> GetRefreshTokenAsync(string refreshToken);
        Task<List<UserRefreshToken>> GetListOfRefreshTokensByUserIdAsync(string userId);
        Task AddRefreshTokenAsync(UserRefreshToken refreshToken);
        Task DeleteRefreshTokenAsync(UserRefreshToken refreshToken);
        Task UpdateRefreshTokenAsync(List<UserRefreshToken> refreshToken);
    }
}
