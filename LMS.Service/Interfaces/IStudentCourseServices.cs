using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Interfaces
{
    public interface IStudentCourseServices
    {
        Task<ResponseModel<List<CourseResponse>>> GetAllEnrollCourse(Guid stdId);
        Task<ResponseModel<StudentCourseResponse>> EnrollInCourse(StudentCourseRequest stdCourseRequest);
    }
}
