using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.RequestModel
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage="Name is Required.")]
        public string? FullName { get; set; }
        
        [EmailAddress]
        [Required(ErrorMessage = "Email is Required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please Select Role.")]
        public int RoleId { get; set; }
    }
}
