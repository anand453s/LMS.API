namespace LMS.Shared.ResponseModel
{
    public class CourseMaterialResponse
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string? MaterialName { get; set; }
        public string? MaterialDesc { get; set; }
        public string? FilePath { get; set; }
    }
}
