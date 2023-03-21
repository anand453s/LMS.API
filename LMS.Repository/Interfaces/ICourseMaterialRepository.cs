using LMS.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Interfaces
{
    public interface ICourseMaterialRepository
    {
        Task<int> AddNewCourseMaterial(CourseMaterial courseMaterial);
        Task<CourseMaterial> GetCourseMaterialById(Guid cmId);
        Task<List<CourseMaterial>> GetCourseMaterialListByCourseId(Guid courseId);
        Task<int> UpdateCourseMaterial(CourseMaterial courseMaterial);
    }
}
