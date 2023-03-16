using LMS.Repository.Entities;
using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Interfaces
{
    public interface ILoginService
    {
        Task<ResponseModel<List<RoleType>>> GetRoleType();
        Task<ResponseModel<UserRegisterResponse>> UserRegister(UserRegisterRequest loginReq);
        Task<ResponseModel<LoginResponse>> ValidateUserLogin(LoginRequest loginReq);
    }
}
