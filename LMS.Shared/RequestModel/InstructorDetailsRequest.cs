using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.RequestModel
{
    public class InstructorDetailsRequest
    {
        [Required(ErrorMessage = "LoginId is Required.")]
        public Guid LoginId { get; set; }
        public long Mobile { get; set; }
        public string? ProfilePic { get; set; }
        public string? Gender { get; set; }
        public string? Specialization { get; set; }
        public int Experience { get; set; }
    }
}
