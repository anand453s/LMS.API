using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.RequestModel
{
    public class CourseRequest
    {
        public Guid CourseId { get; set; }

        [Required(ErrorMessage = "Course Name is Required.")]
        public string? CourseName { get; set; }
        public string? CourseDesc { get; set; }

        [Required(ErrorMessage = "CreatedBy_InstId is Required.")]
        public Guid CreatedBy_InstId { get; set; }
        public int CourseCapacity { get; set; }
    }
}
