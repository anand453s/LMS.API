using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;

namespace LMS.Service.Interfaces
{
    public interface ICourseServices
    {
        Task<ResponseModel<CourseResponse>> AddCourse(CourseRequest courseReq);
        Task<ResponseModel<List<CourseResponse>>> AllPublishedCourseList();
        Task<ResponseModel<List<CourseResponse>>> AllCourseOfInst(Guid instId);
    }
}
