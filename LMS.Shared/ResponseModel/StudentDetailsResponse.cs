namespace LMS.Shared.ResponseModel
{
    public class StudentDetailsResponse
    {
        public Guid StudentId { get; set; }
        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? ProfilePicPath { get; set; }
        public long Mobile { get; set; }
        public string? Gender { get; set; }
        public string? College { get; set; }
        public string? Address { get; set; }
        public bool IsActice { get; set; }
        public bool IsDeleated { get; set; }
    }
}
