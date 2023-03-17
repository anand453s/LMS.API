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
        private readonly IStudentService _studentService;
        private readonly IInstructorServices _instructorServices;

        public LoginService(IUserLoginRepository userLoginRepository, IStudentService studentService, IInstructorServices instructorServices)
        {
            _userLoginRepository = userLoginRepository;
            _studentService = studentService;
            _instructorServices = instructorServices;
        }

        public async Task<ResponseModel<List<RoleType>>> GetRoleType()
        {
            var response = new ResponseModel<List<RoleType>>();
            var userRolesList = await _userLoginRepository.GetAllRoles();
            var result = userRolesList.Where(x => x.RoleName != "Admin").ToList();
            if(result.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "List of all users role type";
                response.Data = result;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "No User Role Defined.";
            }
            return response;
        }

        public async Task<ResponseModel<UserRegisterResponse>> UserRegister(UserRegisterRequest registerReq)
        {
            var response = new ResponseModel<UserRegisterResponse>();
            var allUsers = await _userLoginRepository.GetAllUsers();
            var emailExists = allUsers.Any(x => x.Email.ToLower() == registerReq.Email.ToLower());
            if (registerReq.RoleId != 2 && registerReq.RoleId != 3)
            {
                response.IsSuccess = false;
                response.Message = "Role is required.";
            }
            else if (emailExists)
            {
                var registeredUser = allUsers.Where(x => x.Email.ToLower() == registerReq.Email.ToLower()).FirstOrDefault();
                if(registeredUser.IsDeleted == true)
                {
                    response.IsSuccess = false;
                    response.Message = "This Email is already registered, but account is deleated by Admin.";
                }
                else
                {
                    if(registeredUser.IsActive == false)
                    {
                        response.IsSuccess = false;
                        response.Message = "This Email is already registered, but account is blocked by Admin.";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "You are already registered, Please Login.";
                    }
                }
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
                    int res = 0;
                    if(newLogin.RoleId == 3)
                    {
                        res = await _studentService.AddStudent(newLogin.Id);
                    }
                    else
                    {
                        res = await _instructorServices.AddInstructor(newLogin.Id);
                    }
                    if(res > 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Registered Successfully!";
                        response.Data = new UserRegisterResponse
                        {
                            FullName = newLogin.FullName,
                            Email = newLogin.Email,
                            Password = newLogin.Password,
                            RoleType = await _userLoginRepository.GetRoleTypeByRoleId(newLogin.RoleId),
                        };
                    }
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
            var allUsers = await _userLoginRepository.GetAllUsers();
            var isExists = allUsers.Any(x => x.Email.ToLower() == loginReq.Email.ToLower());
            if (isExists)
            {
                var user = allUsers.Where(x => x.Email.ToLower() == loginReq.Email.ToLower()).FirstOrDefault();
                if(user.Password != loginReq.Password)
                {
                    response.IsSuccess = false;
                    response.Message = "Email or Password is incorrect.";
                }
                else if (user.IsDeleted == true)
                {
                    response.IsSuccess = false;
                    response.Message = "Your account is deleated by Admin.";
                }
                else
                {
                    if (user.IsActive == false)
                    {
                        response.IsSuccess = false;
                        response.Message = "Your Account is blocked by Admin.";
                    }
                    else
                    {
                        var roleType = await _userLoginRepository.GetAllRoles();
                        response.IsSuccess = true;
                        response.Message = "Login Successfully.";
                        response.Data = new LoginResponse
                        {
                            UserId = user.Id,
                            FullName = user.FullName,
                            Email = user.Email,
                            RoleId = user.RoleId,
                            RoleType = roleType.Where(x => x.Id == user.RoleId).First().RoleName
                        };
                    }
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "You are not registered with us please register your account.";
            }
            return response;
        }
    }
}
