using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.RequestModel
{
    public class StudentDetailsRequest
    {
        [Required(ErrorMessage = "LoginId is Required.")]
        public Guid UserId { get; set; }
        public long Mobile { get; set; }
        public string? ProfilePic { get; set; }
        public string? Gender { get; set; }
        public string? College { get; set; }
        public string? Address { get; set; }
    }
}