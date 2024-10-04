using System.ComponentModel.DataAnnotations;

namespace NanhiDuniya.Services.AuthAPI.Models.Dto
{
    public class RevokeRefreshTokenDto
    {
        [Required(ErrorMessage = "User ID is required.")]
        [StringLength(50, ErrorMessage = "User ID cannot exceed 50 characters.")]
        public string UserId { get; set; }
    }
}
