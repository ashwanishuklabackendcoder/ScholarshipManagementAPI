using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.SponsorshipMatrix;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Ngo;

namespace ScholarshipManagementAPI.Controllers.Ngo
{
    [ApiController]
    [Route("api/ngo/sponsorship-matrix")]
    public class SponsorshipMatrixController : ControllerBase
    {
        private readonly ISponsorshipMatrixService _service;

        public SponsorshipMatrixController(ISponsorshipMatrixService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMatrix()
        {
            var result = await _service.GetMatrixAsync();

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = "Data fetched successfully"
            });
        }

        [HttpPost("toggle")]
        [Authorize]
        public async Task<IActionResult> Toggle([FromBody] SponsorshipMatrixToggleRequestDto dto)
        {
            var loginId = JwtClaimHelper.LoginId(User);

            var result = await _service.ToggleAsync(dto, loginId);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = "Mapping updated successfully"
            });
        }


    }
}

