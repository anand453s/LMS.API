using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Repository.Entities;
using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;

namespace LMS.Service.Interfaces
{
    public interface IStudentService
    {
        Task<ResponseModel<StudentDetailsResponse>> GetStudentByLoginId(Guid id);
        Task<ResponseModel<StudentDetailsResponse>> AddStudent(StudentDetailsRequest stdReq);
        Task<ResponseModel<StudentDetailsResponse>> UpdateStudent(StudentDetailsRequest updateReq);

        //ResponseModel<IEnumerable<Course>> MyCourseList(Guid loginId);
    }
}
