using System.ComponentModel.DataAnnotations;

namespace NanhiDuniya.Services.AuthAPI.Models.Dto
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}
