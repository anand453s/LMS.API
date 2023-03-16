using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseMaterialController : ControllerBase
    {
        private readonly ICourseMaterialService _courseMaterialService;

        public CourseMaterialController(ICourseMaterialService courseMaterialService)
        {
            _courseMaterialService = courseMaterialService;
        }

        [HttpPost]
        [Route("AddCourseMaterial")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> PostAddCourseMaterial([FromForm] CourseMaterialRequest courseReq)
        {
            var result = await _courseMaterialService.AddCourseMaterial(courseReq);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("GetCourseMaterial")]
        [Authorize(Roles = "Admin,Instructor,Student")]
        public async Task<IActionResult> GetCourseMaterial(Guid courseId)
        {
            var result = await _courseMaterialService.GetCourseMaterial(courseId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //[HttpGet]
        //[Route("GetCourseMaterial")]
        //[Authorize(Roles = "Admin,Instructor")]
    }
}
