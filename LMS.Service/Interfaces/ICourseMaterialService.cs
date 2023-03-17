using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;

namespace LMS.Service.Interfaces
{
    public interface ICourseMaterialService
    {
        Task<ResponseModel<string>> AddCourseMaterial(CourseMaterialRequest courseMaterialRequest);
        Task<ResponseModel<List<CourseMaterialResponse>>> GetCourseMaterial(Guid courseId);
        Task<ResponseModel<string>> DeleteCourseMaterial(Guid cmId);
    }
}
