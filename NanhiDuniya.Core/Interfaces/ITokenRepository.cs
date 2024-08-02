using NanhiDuniya.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Interfaces
{
    public interface ITokenRepository
    {
        Task<UserRefreshToken> GetRefreshTokenAsync(string userId, string refreshToken);
        Task<List<UserRefreshToken>> GetListOfRefreshTokensByUserIdAsync(string userId);
        Task AddRefreshTokenAsync(UserRefreshToken refreshToken);
        Task DeleteRefreshTokenAsync(UserRefreshToken refreshToken);
        Task UpdateRefreshTokenAsync(UserRefreshToken refreshToken);
    }
}
