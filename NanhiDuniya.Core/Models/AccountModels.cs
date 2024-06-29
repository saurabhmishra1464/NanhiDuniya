﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Models
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class LoginModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class LoginResponse
    {
        public string? UserID { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }

    public class ForgotPasswordModel
    {
        public string? Email { get; set; }
    }

    public class ResetPasswordModel
    {
        public string? UserId { get; set; }
        public string? Code { get; set; }
        public string? Password { get; set; }
    }
}
