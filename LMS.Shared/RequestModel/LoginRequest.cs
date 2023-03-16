using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.RequestModel
{
    public class LoginRequest
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is Required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        public string? Password { get; set; }
    }
}
