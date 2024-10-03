using Microsoft.EntityFrameworkCore;
using NanhiDuniya.Services.AuthAPI.Data.IRepositories;
using NanhiDuniya.Services.AuthAPI.Models;

namespace NanhiDuniya.Services.AuthAPI.Data.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly NanhiDuniyaDbContext _dbcontext;
        public TokenRepository(NanhiDuniyaDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public async Task<UserRefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            return await _dbcontext.UserRefreshTokens.FirstOrDefaultAsync(rt => rt.RefreshToken == refreshToken);
        }

        public async Task<List<UserRefreshToken>> GetListOfRefreshTokensByUserIdAsync(string userId)
        {
            return await _dbcontext.UserRefreshTokens.Where(rt => rt.UserId == userId && !rt.IsRevoked).ToListAsync();
        }

        public async Task AddRefreshTokenAsync(UserRefreshToken refreshToken)
        {
            await _dbcontext.UserRefreshTokens.AddAsync(refreshToken);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteRefreshTokenAsync(UserRefreshToken refreshToken)
        {
            _dbcontext.UserRefreshTokens.Attach(refreshToken);
            _dbcontext.UserRefreshTokens.Remove(refreshToken);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task UpdateRefreshTokenAsync(List<UserRefreshToken> refreshToken)
        {
            _dbcontext.UserRefreshTokens.UpdateRange(refreshToken);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
