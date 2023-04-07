using LMS.API.Security;
using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ITokenManager _tokenManager;

        public LoginController(ILoginService loginService, ITokenManager tokenManager)
        {
            _loginService = loginService;
            _tokenManager = tokenManager;
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
        public async Task<IActionResult> RegisterUser(UserRegisterRequest registerReq)
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
        public async Task<IActionResult> LoginUser(LoginRequest loginReq)
        {
            var response = await _loginService.ValidateUserLogin(loginReq);
            if (response.IsSuccess)
            {
                var result = _tokenManager.GenerateToken(response);
                if (result.IsSuccess)
                {
                    return Ok(response);
                }
                else
                    return BadRequest(response);
            }
            return NotFound(response);
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                //await HttpContext.SignOutAsync();
                //await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);

                
                return NoContent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
