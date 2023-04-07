using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Instructor")]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorServices _instructorServices;
        public InstructorController(IInstructorServices instructorServices)
        {
            _instructorServices = instructorServices;
        }


        [HttpPut]
        [Route("UpdateInstructorDetails")]
        public async Task<IActionResult> UpdateInstructorDetails( InstructorDetailsRequest updateReq)
        {
            var result = await _instructorServices.UpdateInstructor(updateReq);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet]
        [Route("GetInstructorByLoginId")]
        public async Task<IActionResult> GetInstructorByLoginId(Guid UserId)
        {
            var result = await _instructorServices.GetInstructorByLoginId(UserId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
