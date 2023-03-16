namespace LMS.Shared.ResponseModel
{
    public class LoginResponse
    {
        public Guid? UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public string? RoleType { get; set; }
        public string? Token { get; set; }
    }
}
