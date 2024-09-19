using NanhiDuniya.Core.Resources.AccountDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Resources.ResponseDtos
{

    public class LoginResponse
    {
        public UserProfile User { get; set; }
        //public bool IsEmailConfirmed { get; set; }
    }

    public class UploadImageResponse
    {
        public string? ProfilePictureUrl { get; set; }
    }

    public class UserProfile
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public List<string>? Roles { get; set; }
    }
}
