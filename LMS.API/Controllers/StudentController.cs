using Microsoft.AspNetCore.Mvc;
using LMS.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using LMS.Shared.RequestModel;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService registrationService)
        {
            _studentService = registrationService;
        }


        [HttpPut]
        [Route("UpdateStudentDetails")]
        public async Task<IActionResult> UpdateStudentDetails( StudentDetailsRequest updateReq)
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
