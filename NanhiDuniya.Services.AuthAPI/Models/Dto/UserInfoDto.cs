﻿using System.ComponentModel.DataAnnotations;

namespace NanhiDuniya.Services.AuthAPI.Models.Dto
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
        public string? ProfilePictureUrl { get; set; }
        public List<string>? Roles { get; set; }

    }
}
