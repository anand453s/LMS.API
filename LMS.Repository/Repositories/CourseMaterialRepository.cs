using LMS.Repository.EF.Context;
using LMS.Repository.Entities;
using LMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository.Repositories
{
    public class CourseMaterialRepository : BaseRepository<CourseMaterialRepository>, ICourseMaterialRepository
    {
        readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        int i = 0;
        public CourseMaterialRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = (AppDbContext)_unitOfWork.Db;
        }

        public async Task<int> AddNewCourseMaterial(CourseMaterial courseMaterial)
        {
            await _context.courseMaterials.AddAsync(courseMaterial);
            return await _context.SaveChangesAsync();
        }

        public async Task<CourseMaterial> GetCourseMaterialById(Guid cmId)
        {
            return await _context.courseMaterials.FindAsync(cmId);
        }

        public async Task<List<CourseMaterial>> GetCourseMaterialListByCourseId(Guid courseId)
        {
            return await _context.courseMaterials.Where(x => x.CourseId == courseId).Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<int> UpdateCourseMaterial(CourseMaterial courseMaterial)
        {
            _context.Update(courseMaterial);
            return await _context.SaveChangesAsync();
        }
    }
}
