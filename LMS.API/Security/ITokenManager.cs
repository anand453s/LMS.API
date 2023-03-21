using LMS.Shared;
using LMS.Shared.ResponseModel;

namespace LMS.API.Security
{
    public interface ITokenManager
    {
        ResponseModel<LoginResponse> GenerateToken(ResponseModel<LoginResponse> userLoginDetails);
        //RefreshTokenModel GenerateRefreshToken();

    }
}
