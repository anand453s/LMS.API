using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Interfaces
{
    public interface IAdminService
    {
        Task<ResponseModel<List<CourseResponse>>> GetAllCourse(RequestParameter reqParameter);
        Task<ResponseModel<string>> PublishCourse(Guid courseId);
        Task<ResponseModel<List<StudentDetailsResponse>>> GetAllStudents(RequestParameter reqParameter);
        Task<ResponseModel<List<InstructorDetailsResponse>>> GetAllInstructors(RequestParameter reqParameter);
        Task<ResponseModel<string>> BlockUser(Guid userId);
        Task<ResponseModel<string>> UnblockUser(Guid userId);
    }
}
