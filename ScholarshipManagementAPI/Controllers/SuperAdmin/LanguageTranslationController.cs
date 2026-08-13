using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.LanguageTranslation;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Controllers.SuperAdmin
{
    [ApiController]
    [Route("api/superadmin/language-translations")]
    public class LanguageTranslationController : ControllerBase
    {
        private readonly ILanguageTranslationService _service;

        public LanguageTranslationController(ILanguageTranslationService service)
        {
            _service = service;
        }

        // -------- CREATE --------
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(LanguageTranslationRequestDto dto)
        {
            dto.CreatedBy = JwtClaimHelper.LoginId(User);            // or from claims
            var id = await _service.CreateAsync(dto);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Language translation created successfully"
            });
        }


        // -------- UPDATE --------
        [HttpPut("update/{id:long}")]
        [Authorize]
        public async Task<IActionResult> Update(long id, [FromBody] LanguageTranslationRequestDto dto)
        {
            dto.TranslationId = id;
            dto.UpdatedBy = JwtClaimHelper.LoginId(User);            // or from claims
            var updated = await _service.UpdateAsync(dto);

            if (!updated)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Record not found",
                    Result = null,
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Language translation updated successfully",
                Result = updated,
            });
        }


        // -------- DELETE --------
        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
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
                Message = "Language translation deleted successfully",
                Result = deleted
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
        public async Task<IActionResult> GetByFilter(LanguageTranslationFilterDto filter)
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



        // -------- TRANSLATION MANAGEMENT --------
        [HttpPost("management/search")]
        [Authorize]
        public async Task<IActionResult> GetManagement(LanguageTranslationFilterDto filter)
        {
            var result = await _service.GetManagementAsync(filter);

            return Ok(new ApiResponseDto
            {
                Success = result.Items.Count > 0,
                Result = result,
                Message = result.Items.Count == 0
                    ? "Data not found"
                    : "Data fetched successfully"
            });
        }


    }
}
