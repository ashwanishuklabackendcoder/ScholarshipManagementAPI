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
        private readonly CurrentUserContextService _currentUser;

        public ProgramRegistrationWindowController(IProgramRegistrationWindowService service, CurrentUserContextService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }

        [HttpGet("{programId:long}")]
        public async Task<IActionResult> GetByProgramId(long programId)
        {
            var currentUser = await _currentUser.GetCurrentUserAsync();
            var result = await _service.GetByProgramIdAsync(programId, currentUser);

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
            var currentUser = await _currentUser.GetCurrentUserAsync();
            var id = await _service.SaveAsync(dto, currentUser);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Created successfully"
            });
        }


    }
}
