namespace LMS.Shared.ResponseModel
{
    public class CourseResponse
    {
        public Guid CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? CourseDesc { get; set; }
        public int CourseCapacity { get; set; }
        public Guid CreatedByID { get; set; }
        public string? CreatedByName { get; set; }
        public bool? IsPublish { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifyOn { get; set; }
    }
}
