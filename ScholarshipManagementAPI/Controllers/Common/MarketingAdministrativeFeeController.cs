using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.MarketingAdministrativeFee;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScholarshipManagementAPI.Controllers.Common
{
    [ApiController]
    [Route("api/common/marketing-administrative-fee")]
    public class MarketingAdministrativeFeeController : ControllerBase
    {
        private readonly IMarketingAdministrativeFeeService _service;
        private readonly CurrentUserContextService _currentUser;

        public MarketingAdministrativeFeeController(
            IMarketingAdministrativeFeeService service,
            CurrentUserContextService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<IActionResult> GetCurrent()
        {
            var result = await _service.GetCurrentAsync();

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = "Marketing administrative fee fetched successfully."
            });

        }



        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] MarketingAdministrativeFeeRequestDto dto)
        {
            var currentUser = await _currentUser.GetCurrentUserAsync();
            var result = await _service.UpdateAsync(dto, currentUser);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = "Marketing administrative fee updated successfully."
            });     
        }



        [HttpGet("history")]
        [Authorize]
        public async Task<IActionResult> GetHistory()
        {
            var result = await _service.GetHistoryAsync();

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = "Marketing administrative fee history fetched successfully."
            });
        }


    }
}

