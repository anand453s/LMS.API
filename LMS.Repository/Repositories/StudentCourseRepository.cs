using LMS.Repository.EF.Context;
using LMS.Repository.Entities;
using LMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository.Repositories
{
    public class StudentCourseRepository : BaseRepository<StudentCourseRepository>, IStudentCourseRepository
    {
        readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        int i = 0;
        public StudentCourseRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = (AppDbContext)_unitOfWork.Db;
        }

        public async Task<int> AddStudentCourse(StudentCourse stdCourse)
        {
            _context.studentCourses.Add(stdCourse);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<StudentCourse>> GetAllCourses()
        {
            return await _context.studentCourses.ToListAsync();
        }
        public async Task<bool> IsCourseEnrolled(Guid stdId, Guid courseID)
        {
            return await _context.studentCourses.AnyAsync(x => x.StudentId == stdId && x.CourseId == courseID);
        }

        public async Task<int> TotalEnrolled(Guid courseID)
        {
            return await _context.studentCourses.Where(x => x.CourseId == courseID).CountAsync();
        }
    }
}
