using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.CurrencyConversion;
using ScholarshipManagementAPI.DTOs.SuperAdmin.GeneralSettings;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Controllers.SuperAdmin
{
    [ApiController]
    [Route("api/superadmin/currency-conversion")]
    public class CurrencyConversionController : ControllerBase
    {
        private readonly ICurrencyConversionService _service;

        public CurrencyConversionController(ICurrencyConversionService service)
        {
            _service = service;
        }


        [HttpGet("sync")]
        [Authorize]
        public async Task<IActionResult> SyncRates()
        {
            var result = await _service.SyncRatesAsync("API_TEST");

            return Ok(new
            {
                success = true,
                message = "Currency sync completed",
                result
            });
        }



        // -------- CREATE --------
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(CurrencyConversionRequestDto dto)
        {

            dto.CreatedDate = DateTime.UtcNow;                            // always server-side
            dto.CreatedBy = JwtClaimHelper.UserName(User).ToString();      // or from claims

            var id = await _service.AddManualRate(dto);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Currency conversion created successfully"
            });
        }


        // -------- GET latest rates --------
        [HttpGet("latest-rates")]
        [Authorize]
        public async Task<IActionResult> GetLatestRates()
        {
            var data = await _service.GetCurrentCurrencyRateAsync();

            if (data == null || !data.Any())
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Record not found",
                    Result = null
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = data,
                Message = "Record found"
            });
        }


        // -------- GET BY ID --------
        [HttpGet("getById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(long id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Record not found",
                    Result = null
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = data,
                Message = "Record found"
            });
        }


        // -------- FILTER / GET ALL --------
        [HttpPost("search")]
        [Authorize]
        public async Task<IActionResult> GetByFilter(CurrencyConversionFilterDto filter)
        {
            var result = await _service.GetByFilterAsync(filter);

            return Ok(new ApiResponseDto
            {
                Success = result.Items.Count == 0 ? false : true,
                Result = result,
                Message = result.Items.Count == 0
                    ? "Data not found"
                    : "Data fetched successfully"
            });
        }


    }
}
