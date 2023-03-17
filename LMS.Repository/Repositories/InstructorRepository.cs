using LMS.Repository.EF.Context;
using LMS.Repository.Entities;
using LMS.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository.Repositories
{
    public class InstructorRepository : BaseRepository<InstructorRepository>, IInstructorRepository
    {
        readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        int i = 0;
        public InstructorRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = (AppDbContext)_unitOfWork.Db;
        }

        public async Task<int> AddNewInstructor(InstructorDetails instructorDetails)
        {
            await _context.instructors.AddAsync(instructorDetails);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateInstructor(InstructorDetails instructorDetails)
        {
            _context.Update(instructorDetails);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<InstructorDetails>> GetAllInstructors()
        {
            return await _context.instructors.ToListAsync();
        }
    }
}
