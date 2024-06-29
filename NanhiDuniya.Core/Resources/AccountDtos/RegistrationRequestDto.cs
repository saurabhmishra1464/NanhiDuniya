using Microsoft.AspNetCore.Http;
using NanhiDuniya.Core.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Resources.AccountDtos
{
    public class RegistrationRequestDto
    {

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [PhoneNumber(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [PasswordComplexity(ErrorMessage = "Password must be at least 8 characters long, contain an uppercase letter, a lowercase letter, a number, and a special character.")]
        public string Password { get; set; }
    }

}
