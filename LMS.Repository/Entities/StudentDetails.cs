using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Entities
{
    [Table("Student")]
    public class StudentDetails : BaseModel<Guid>
    {
        [Display(Name = "UserLogin")]
        public Guid LoginId { get; set; }

        [ForeignKey("LoginId")]
        public virtual UserLogin UserLogin { get; set; }

        public long Mobile { get; set; }

        [StringLength(500)]
        public string? ProfilePicPath { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(100)]
        public string? College { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }
    }
}
