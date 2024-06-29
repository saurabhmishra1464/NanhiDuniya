using Microsoft.AspNetCore.Http;
using NanhiDuniya.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponse> Login(LoginModel model);
        Task<ResultResponse> Register(RegisterModel model);
    }
}
