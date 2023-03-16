using Microsoft.AspNetCore.Mvc;
using LMS.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using LMS.Shared.RequestModel;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize(Roles = "Admin,Student")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService registrationService)
        {
            _studentService = registrationService;
        }


        //[HttpPost]
        //[Route("SaveStudentDetails")]
        //public async Task<IActionResult> PostAddStudentDetails([FromForm] StudentDetailsRequest regRequest)
        //{
        //    var result = await _studentService.AddStudent(regRequest);
        //    if (result.IsSuccess)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result);
        //}

        [HttpPut]
        [Route("UpdateStudentDetails")]
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
        public async Task<IActionResult> GetStudentByLoginId(Guid loginId)
        {
            var result = await _studentService.GetStudentByLoginId(loginId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
