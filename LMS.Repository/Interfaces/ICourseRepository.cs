using LMS.Repository.Entities;

namespace LMS.Repository.Interfaces
{
    public interface ICourseRepository
    {
        Task<int> AddNewCourse(Course course);
        Task<List<Course>> GetAllCourses();
        Task<Course> GetCourseById(Guid courseId);
    }
}
