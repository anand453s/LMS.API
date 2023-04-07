using LMS.Repository.EF.Context;
using LMS.Repository.Entities;
using LMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository.Repositories
{
    public class CourseRepository : BaseRepository<CourseRepository>, ICourseRepository
    {
        readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        int i = 0;
        public CourseRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = (AppDbContext)_unitOfWork.Db;
        }

        public async Task<int> AddNewCourse(Course course)
        {
            await _context.courses.AddAsync(course);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Course>> GetAllCoursesOfInst(Guid instId)
        {
            return await _context.courses.Include(x => x.Instructor).ThenInclude(y => y.UserLogin).Where(x => x.CreatedBy == instId && !x.IsDeleted).ToListAsync();
        }

        public async Task<List<Course>> GetAllCourses()
        {
            return await _context.courses.Include(x => x.Instructor).ThenInclude(x => x.UserLogin).Where(y => y.IsDeleted == false).ToListAsync();
        }

        public async Task<List<Course>> GetAllPublishedCourse()
        {
            return await _context.courses.Include(x => x.Instructor).ThenInclude(y => y.UserLogin).Where(z => z.IsPublish && z.IsActive && !z.IsDeleted).ToListAsync();
        }

        public async Task<Course> GetCourseById(Guid courseId)
        {
            return await _context.courses.Include(x => x.Instructor).ThenInclude(y => y.UserLogin).Where(x => x.Id == courseId).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateCourse(Course course)
        {
            _context.Update(course);
            return await _context.SaveChangesAsync();
        }
    }
}
