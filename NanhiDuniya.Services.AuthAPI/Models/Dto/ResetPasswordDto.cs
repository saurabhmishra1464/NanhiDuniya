using System.ComponentModel.DataAnnotations;

namespace NanhiDuniya.Services.AuthAPI.Models.Dto
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Token is required.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
