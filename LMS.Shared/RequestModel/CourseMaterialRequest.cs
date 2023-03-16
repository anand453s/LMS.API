using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.RequestModel
{
    public class CourseMaterialRequest
    {
        [Required(ErrorMessage = "CourseId is Required.")]
        public Guid CourseId { get; set; }
        public string? MaterialName { get; set; }
        public string? MaterialDesc { get; set; }
        public string? File_Base64 { get; set; }
    }
}
