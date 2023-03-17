using Microsoft.AspNetCore.Mvc;
using LMS.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using LMS.Shared.RequestModel;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService registrationService)
        {
            _studentService = registrationService;
        }


        [HttpPut]
        [Route("UpdateStudentDetails")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> UpdateStudentDetails([FromForm] StudentDetailsRequest updateReq)
        {
            var result = await _studentService.UpdateStudent(updateReq);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet]
        [Route("GetStudentByLoginId")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> GetStudentByLoginId(Guid UserId)
        {
            var result = await _studentService.GetStudentByLoginId(UserId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
