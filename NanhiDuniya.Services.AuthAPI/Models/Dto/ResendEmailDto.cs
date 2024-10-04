using System.ComponentModel.DataAnnotations;

namespace NanhiDuniya.Services.AuthAPI.Models.Dto
{
    public class ResendEmailDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}
