using Microsoft.EntityFrameworkCore;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Data.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly NanhiDuniyaDbContext _dbcontext;
        public TokenRepository(NanhiDuniyaDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public async Task<UserRefreshToken> GetRefreshTokenAsync(string userId, string refreshToken)
        {
            return await _dbcontext.UserRefreshTokens.FirstOrDefaultAsync(rt=>rt.UserId==userId && rt.RefreshToken== refreshToken);
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
