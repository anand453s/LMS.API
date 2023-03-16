using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Entities
{
    [Table("StudentCourse")]
    public class StudentCourse : BaseModel<Guid>
    {
        [Display(Name = "StudentDetails")]
        public Guid StudentId { get; set; }

        [ForeignKey("StudentId")]
        public virtual StudentDetails StudentDetails { get; set; }

        [Display(Name = "Course")]
        public Guid CourseId { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
    }
}
