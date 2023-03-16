using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize(Roles = "Admin,Instructor")]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorServices _instructorServices;
        public InstructorController(IInstructorServices instructorServices)
        {
            _instructorServices = instructorServices;
        }


        [HttpPost]
        [Route("SaveInstructorDetails")]
        public async Task<IActionResult> PostAddInstructorDetails([FromForm] InstructorDetailsRequest regRequest)
        {
            var result = await _instructorServices.AddInstructor(regRequest);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPut]
        [Route("UpdateInstructorDetails")]
        public async Task<IActionResult> UpdateInstructorDetails([FromForm] InstructorDetailsRequest updateReq)
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
        public async Task<IActionResult> GetInstructorByLoginId(Guid loginId)
        {
            var result = await _instructorServices.GetInstructorByLoginId(loginId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
