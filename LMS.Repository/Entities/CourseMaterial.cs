using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Entities
{
    [Table("CourseMaterial")]
    public class CourseMaterial : BaseModel<Guid>
    {
        [Display(Name = "Course")]
        public Guid CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [StringLength(100)]
        public string? MaterialName { get; set; }

        [StringLength(500)]
        public string? MaterialDesc { get; set; }

        [StringLength(500)]
        public string? FilePath { get; set; }
    }
}
