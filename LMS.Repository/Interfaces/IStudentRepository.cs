using LMS.Repository.Entities;
using LMS.Shared.ResponseModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Interfaces
{
    public interface IStudentRepository
    {
        Task<int> AddNewStudent(StudentDetails studentDetails);
        Task<int> UpdateStudent(StudentDetails studentDetails);
        Task<List<StudentDetails>> GetAllStudents();
        Task<StudentDetails> GetStudentByLoginId(Guid userId);
    }
}
