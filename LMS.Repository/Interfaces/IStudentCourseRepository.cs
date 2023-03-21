using LMS.Repository.Entities;

namespace LMS.Repository.Interfaces
{
    public interface IStudentCourseRepository
    {
        Task<int> AddStudentCourse(StudentCourse stdCourse);
        Task<bool> AnyCourseInTable(Guid courseId);
        Task<bool> IsStdEnrolledInCourse(Guid stdId, Guid courseID);
        Task<List<StudentCourse>> GetEnrolledCourseOfStudent(Guid studentId);
        Task<int> TotalEnrolled(Guid courseID);
    }
}
