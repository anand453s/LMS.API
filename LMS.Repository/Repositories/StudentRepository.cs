using LMS.Repository.EF.Context;
using LMS.Repository.Entities;
using LMS.Repository.Interfaces;
using LMS.Shared.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository.Repositories
{
    public class StudentRepository : BaseRepository<StudentRepository>, IStudentRepository
    {
        readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        public StudentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = (AppDbContext)_unitOfWork.Db;
        }

        public async Task<int> AddNewStudent(StudentDetails studentDetails)
        {
            await _context.students.AddAsync(studentDetails);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateStudent(StudentDetails studentDetails)
        {
            _context.Update(studentDetails);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<StudentDetails>> GetAllStudents()
        {
            return await _context.students.ToListAsync();
        }
    }
}
