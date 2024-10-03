using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Models.Admin
{
    public class AdminModel
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public DateTime? Birthday { get; set; }
        [MaxLength(10)]
        public string Gender { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(5)]
        public string BloodGroup { get; set; }

    }
}
