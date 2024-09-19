using NanhiDuniya.Core.Resources.AccountDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Interfaces
{
    public interface IUserService
    {
        string GenerateVerifyEmailLink(string email, string token);
        string GenerateResetPasswordLink(string email, string token);
    }
}
