using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Models.Student
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required]
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }
        [Required]
        public DateTime EnrollmentDate { get; set; }
    }
}
