using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Service.Services
{
    public class JWTService
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? Secret { get; set; }
        public int AccessTokenExpiry { get; set; }
        public int RefreshTokenExpiry { get; set; }
    }
}
