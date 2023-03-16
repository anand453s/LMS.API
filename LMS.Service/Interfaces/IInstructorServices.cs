using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;

namespace LMS.Service.Interfaces
{
    public interface IInstructorServices
    {
        Task<ResponseModel<InstructorDetailsResponse>> GetInstructorByLoginId(Guid loginId);
        Task<InstructorDetailsResponse> GetInstructorByInstId(Guid instId);
        //Task<ResponseModel<InstructorDetailsResponse>> AddInstructor(InstructorDetailsRequest addReq);
        Task<int> AddInstructor(Guid loginId);
        Task<ResponseModel<InstructorDetailsResponse>> UpdateInstructor(InstructorDetailsRequest updateReq);
    }
}
