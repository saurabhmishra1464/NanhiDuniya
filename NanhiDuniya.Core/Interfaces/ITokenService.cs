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
        Task<String> GenerateRefreshToken(string userId);
        Task<string> GenerateJWT(IEnumerable<Claim> Claims);

    }
}
