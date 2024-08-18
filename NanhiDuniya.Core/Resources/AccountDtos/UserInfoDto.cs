using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Resources.AccountDtos
{
    public class UserInfoDto
    {
        [Required(ErrorMessage = "User Id is required")]
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? UserName { get; set; }
        public string PhoneNumber { get; set; }
        [EmailAddress(ErrorMessage = "Email Id is required")]
        public string Email { get; set; }
        public string? Bio { get; set; }
    }
}
