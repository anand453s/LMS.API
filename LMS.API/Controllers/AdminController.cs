using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        [Route("GetAllCourse")]
        public async Task<IActionResult> GetAllCourse([FromQuery] RequestParameter reqParameter)
        {
            var result = await _adminService.GetAllCourse(reqParameter);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        [Route("PublishCourse")]
        public async Task<IActionResult> PublishCourse(Guid courseId)
        {
            var result = await _adminService.PublishCourse(courseId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        [Route("BlockCourse")]
        public async Task<IActionResult> BlockCourse(Guid courseId)
        {
            var result = await _adminService.BlockCourse(courseId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents([FromQuery] RequestParameter reqParameter)
        {
            var result = await _adminService.GetAllStudents(reqParameter);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("GetAllInstructors")]
        public async Task<IActionResult> GetAllInstructors([FromQuery] RequestParameter reqParameter)
        {
            var result = await _adminService.GetAllInstructors(reqParameter);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
