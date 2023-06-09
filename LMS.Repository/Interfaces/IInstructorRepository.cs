﻿using LMS.Repository.Entities;
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
        Task<List<InstructorDetails>> GetAllInstructors();
        Task<InstructorDetails> GetInstructorByLoginId(Guid userId);
    }
}
