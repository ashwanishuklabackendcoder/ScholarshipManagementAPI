using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.MasterSchool;
using ScholarshipManagementAPI.Services.Interface.School;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Controllers.School
{
    [ApiController]
    public class SchoolRegistrationController : ControllerBase
    {
        private readonly IMasterSchoolService _schoolService;

        public SchoolRegistrationController(IMasterSchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        // -------- PUBLIC SELF-REGISTRATION --------
        [HttpPost("api/school/register")]
        public async Task<IActionResult> Register([FromBody] MasterSchoolRequestDto dto)
        {
            var id = await _schoolService.CreateAsync(dto);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "School registration submitted successfully."
            });
        }
    }
}
