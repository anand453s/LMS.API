using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.RequestModel
{
    public class StudentCourseRequest
    {
        [Required(ErrorMessage = "Student Id is Required.")]
        public Guid StudentId { get; set; }
        [Required(ErrorMessage = "Course Id is Required.")]
        public Guid CourseId { get; set; }
    }
}
