using LMS.Repository.Entities;

namespace LMS.Repository.Interfaces
{
    public interface ICourseRepository
    {
        Task<int> AddNewCourse(Course course);
        Task<List<Course>> GetAllCourses();
        Task<Course> GetCourseById(Guid courseId);
        Task<List<Course>> GetAllPublishedCourse();
        Task<int> UpdateCourse(Course course);
        Task<List<Course>> GetAllCoursesOfInst(Guid instId);
    }
}
