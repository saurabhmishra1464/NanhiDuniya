using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Resources.AccountDtos
{
    public class VerifyEmailDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
