using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Entities
{
    [Table("Course")]
    public class Course : BaseModel<Guid>
    {
        [StringLength(100)]
        public string CourseName { get; set; }

        [StringLength(500)]
        public string CourseDesc { get; set; }

        [Display(Name = "Instructor")]
        public Guid CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public InstructorDetails Instructor { get; set; }
        public int CourseCapacity { get; set; }
        public bool IsPublish { get; set; }
    }
}
