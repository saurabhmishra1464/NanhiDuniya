using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NanhiDuniya.Services.AuthAPI.Models
{
    public class Admin
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public DateTime? Birthday { get; set; }
        [MaxLength(10)]
        public string Gender { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(5)]
        public string BloodGroup { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
