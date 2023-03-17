using LMS.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        [Route("GetAllCourse")]
        public async Task<IActionResult> GetAllCourse()
        {
            var result = await _adminService.GetAllCourse();
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

    }
}
