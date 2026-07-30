using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.ProgramRegistrationWindow;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.University;

namespace ScholarshipManagementAPI.Controllers.University
{
    [ApiController]
    [Route("api/program-registration-window")]
    public class ProgramRegistrationWindowController : ControllerBase
    {
        private readonly IProgramRegistrationWindowService _service;

        public ProgramRegistrationWindowController(IProgramRegistrationWindowService service)
        {
            _service = service;
        }

        [HttpGet("{programId:long}")]
        public async Task<IActionResult> GetByProgramId(long programId)
        {
            var result = await _service.GetByProgramIdAsync(programId);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = "Retrieved successfully"
            });

        }

        [HttpPost]
        public async Task<IActionResult> Save(
            [FromBody] ProgramRegistrationWindowRequestDto dto)
        {
            var loginId = JwtClaimHelper.LoginId(User);
            var id = await _service.SaveAsync(dto, loginId);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Created successfully"
            });
        }


    }
}
