using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.AcademicRegistration;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.University;

namespace ScholarshipManagementAPI.Controllers.University
{
    [ApiController]
    [Route("api/university/academic-registration")]
    public class AcademicRegistrationController : ControllerBase
    {
        private readonly IStudentAcademicRegistrationService _service;
        private readonly CurrentUserContextService _currentUser;

        public AcademicRegistrationController(IStudentAcademicRegistrationService service, CurrentUserContextService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }


        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] AcademicRegistrationFilterDto filter)
        {
            var result = await _service.SearchAsync(filter, await _currentUser.GetCurrentUserAsync());

            return Ok(new ApiResponseDto
            {
                Success = result.Items.Count == 0 ? false : true,
                Result = result,
                Message = result.Items.Count == 0
                    ? "Data not found"
                    : "Data fetched successfully"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentRequestDto dto)
        {
            var result = await _service.RegisterStudentAsync(dto, await _currentUser.GetCurrentUserAsync());

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = "Student registered successfully."
            });
        }

    }
}
