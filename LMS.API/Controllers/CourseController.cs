using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseServices _courseServices;
        private readonly IStudentCourseServices _studentCourseServices;

        public CourseController(ICourseServices courseServices, IStudentCourseServices studentCourseServices)
        {
            _courseServices = courseServices;
            _studentCourseServices = studentCourseServices;
        }


        [HttpPost]
        [Route("AddCourse")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> PostAddCourse([FromForm] CourseRequest courseReq)
        {
            var result = await _courseServices.AddCourse(courseReq);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPut]
        [Route("UpdateCourse")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UpdateCourse([FromForm] CourseRequest courseReq)
        {
            if(courseReq.CourseId == null)
            {
                return BadRequest("Course Id is required.");
            }
            var result = await _courseServices.UpdateCourse(courseReq);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpDelete]
        [Route("DeleteCourse")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var result = await _courseServices.DeleteCourse(courseId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet]
        [Route("GetAllCourseOfInstructor")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> GetAllCourseOfInstructor(Guid instId)
        {
            var result = await _courseServices.AllCourseOfInst(instId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet]
        [Route("GetCourseDetails")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> GetCourseDetails(Guid courseID)
        {
            var result = await _courseServices.GetCourseById(courseID);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }


        [HttpGet]
        [Route("GetAllPublishedCourse")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> GetAllPublishedCourse()
        {
            var result = await _courseServices.AllPublishedCourseList();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else if (result.IsSuccess && result.Data == null)
            {
                return NoContent();
            }
            return NotFound(result);
        }


        [HttpPost]
        [Route("EnrollInCourse")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> EnrollInCourse([FromForm] StudentCourseRequest stdCourseReq)
        {
            var result = await _studentCourseServices.EnrollInCourse(stdCourseReq);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet]
        [Route("GetAllEnrollCourse")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> GetAllEnrollCourse(Guid stdId)
        {
            var result = await _studentCourseServices.GetAllEnrollCourse(stdId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
