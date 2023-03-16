using LMS.API.Security;
using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IAuthenticationService _authService;
        public LoginController(ILoginService loginService, IAuthenticationService authService)
        {
            _loginService = loginService;
            _authService = authService;

        }

        [HttpGet]
        [Route("GetRoleTypes")]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _loginService.GetRoleType();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else if (result.IsSuccess && result.Data == null)
            {
                return NoContent();
            }
            return BadRequest(result);
        }

        //Save Login Details
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> RegisterUser([FromForm] UserRegisterRequest registerReq)
        {
            var result = await _loginService.UserRegister(registerReq);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //Validate Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUser([FromForm] LoginRequest loginReq)
        {
            var response = await _loginService.ValidateUserLogin(loginReq);
            if (response.IsSuccess)
            {
                var result = _authService.GenerateToken(response);
                if (result.IsSuccess)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            return NotFound(loginReq);
        }
    }
}
