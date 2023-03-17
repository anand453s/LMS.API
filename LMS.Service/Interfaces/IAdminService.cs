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
        Task<ResponseModel<List<CourseResponse>>> GetAllCourse();
        Task<ResponseModel<string>> PublishCourse(Guid courseId);
        Task<ResponseModel<string>> BlockCourse(Guid courseId);
    }
}
