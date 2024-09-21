using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Models
{
    public class UserRefreshToken
    {
        public Guid Id { get; set; }
        public string RefreshToken { get; set; } = null!;
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string? UserId { get; set; }
        public bool IsRevoked { get; set; }
    }
}
