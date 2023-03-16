using LMS.Repository.Entities;
using LMS.Repository.Interfaces;
using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;

namespace LMS.Service.Services
{
    public class LoginService : ILoginService
    {
        int i = 0;
        private readonly IUserLoginRepository _userLoginRepository;
        public LoginService(IUserLoginRepository userLoginRepository)
        {
            _userLoginRepository = userLoginRepository;
        }

        public async Task<ResponseModel<List<RoleType>>> GetRoleType()
        {
            var response = await _userLoginRepository.GetAllUserRoles();
            return new ResponseModel<List<RoleType>>
            {
                IsSuccess = true,
                Message = "User Role Types",
                Data = response
            };
        }

        public async Task<ResponseModel<UserRegisterResponse>> UserRegister(UserRegisterRequest registerReq)
        {
            var response = new ResponseModel<UserRegisterResponse>();
            var isExist = await _userLoginRepository.CheckUserExist(registerReq.Email);
            if (isExist)
            {
                response.IsSuccess = false;
                response.Message = "Email already exists.";
            }else if(registerReq.RoleId != 2 && registerReq.RoleId != 3)
            {
                response.IsSuccess = false;
                response.Message = "Role is required.";
            }
            else
            {
                UserLogin newLogin = new UserLogin
                {
                    Id = Guid.NewGuid(),
                    FullName = registerReq.FullName,
                    Email = registerReq.Email,
                    Password = registerReq.Password,
                    RoleId = registerReq.RoleId,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyOn = DateTime.Now,
                };
                i = await _userLoginRepository.AddUser(newLogin);
                if (i > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "User SignUp successfully!";
                    response.Data = new UserRegisterResponse
                        {
                            FullName = newLogin.FullName,
                            Email = newLogin.Email,
                            Password = newLogin.Password,
                            RoleType = await _userLoginRepository.GetRoleTypeByRoleId(newLogin.RoleId),
                };
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Failed to Register. Please try again.";
                }
            }
            return response;
        }

        public async Task<ResponseModel<LoginResponse>> ValidateUserLogin(LoginRequest loginReq)
        {
            var response = new ResponseModel<LoginResponse>();
            var isExist = await _userLoginRepository.VarifyEmailPassword(loginReq.Email, loginReq.Password);
            if (isExist)
            {
                var userDetails = await _userLoginRepository.GetUserLoginDetails(loginReq.Email);
                response.IsSuccess = true;
                response.Message = "Login Successfully.";
                response.Data = new LoginResponse
                    {
                        LoginId = userDetails.Id,
                        FullName = userDetails.FullName,
                        Email = userDetails.Email,
                        RoleId = userDetails.RoleId,
                        RoleType = await _userLoginRepository.GetRoleTypeByRoleId(userDetails.RoleId)
                };
                }
            else
            {
                response.IsSuccess = false;
                response.Message = "Login details not mathch.";
            };
            return response;
        }
    }
}
