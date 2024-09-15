using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Resources.AccountDtos
{
    public class UserLoginModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<string>? Roles { get; set; }
        public string? UserName { get; set; }
    }
}
