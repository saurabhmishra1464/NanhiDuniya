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

        public UserRefreshToken()
        {
            Expires = DateTime.UtcNow.AddDays(15);
            Created = DateTime.UtcNow;
        }
        public string? UserId { get; set; }
    }
}
