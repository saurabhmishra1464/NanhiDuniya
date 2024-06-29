using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Validators
{
    public class PasswordComplexityAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;
            if (password == null)
            {
                return new ValidationResult("Password is required.");
            }

            var hasUpperCaseLetter = Regex.IsMatch(password, "[A-Z]");
            var hasLowerCaseLetter = Regex.IsMatch(password, "[a-z]");
            var hasDecimalDigit = Regex.IsMatch(password, "[0-9]");
            var hasSpecialCharacter = Regex.IsMatch(password, "[^a-zA-Z0-9]");

            if (password.Length >= 8 && hasUpperCaseLetter && hasLowerCaseLetter && hasDecimalDigit && hasSpecialCharacter)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Password must be at least 8 characters long, contain an uppercase letter, a lowercase letter, a number, and a special character.");
        }
    }
}
