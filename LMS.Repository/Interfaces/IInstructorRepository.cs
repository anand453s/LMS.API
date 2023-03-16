using LMS.Repository.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Interfaces
{
    public interface IInstructorRepository
    {
        Task<int> AddNewInstructor(InstructorDetails instructorDetails);
        Task<int> UpdateInstructor(InstructorDetails instructorDetails);
        Task<InstructorDetails> GetInstructorByInstId(Guid instId);
        Task<InstructorDetails> GetInstructorByLoginId(Guid instId);
    }
}
