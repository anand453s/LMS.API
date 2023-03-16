using LMS.Shared.ResponseModel;

namespace LMS.API.Security
{
    public interface IAuthenticationService
    {
        ResponseModel<LoginResponse> GenerateToken(ResponseModel<LoginResponse> userLoginDetails);
    }
}
