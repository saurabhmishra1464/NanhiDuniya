using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Resources.AccountDtos
{
    public class LoginResponseResource
    {
        public string? UserID { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
