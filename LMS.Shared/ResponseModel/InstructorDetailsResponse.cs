namespace LMS.Shared.ResponseModel
{
    public class InstructorDetailsResponse
    {
        public Guid LoginId { get; set; }
        public Guid InstructorId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? ProfilePicPath { get; set; }
        public long Mobile { get; set; }
        public string? Gender { get; set; }
        public string? Specialization { get; set; }
        public int Experience { get; set; }
    }
}
