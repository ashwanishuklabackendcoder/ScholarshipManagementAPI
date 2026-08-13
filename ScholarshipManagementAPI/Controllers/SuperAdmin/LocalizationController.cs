using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Controllers.SuperAdmin
{
    [ApiController]
    [Route("api/localization")]
    public class LocalizationController : ControllerBase
    {
        private readonly ILocalizationService _service;

        public LocalizationController(ILocalizationService service)
        {
            _service = service;
        }


        // -------- GET TRANSLATIONS BY LANGUAGE --------
        [HttpGet("{languageCode}")]
        [Authorize]
        public async Task<IActionResult> GetTranslations(string languageCode)
        {
            var data = await _service.GetTranslationsAsync(languageCode);

            if (data == null)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Language not found",
                    Result = null
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = data,
                Message = "Translations fetched successfully"
            });
        }


    }
}
