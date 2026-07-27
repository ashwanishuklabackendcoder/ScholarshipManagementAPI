using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.SponsorshipTypes;
using ScholarshipManagementAPI.DTOs.Ngo.StudentCategory;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Ngo;

namespace ScholarshipManagementAPI.Controllers.Ngo
{
    [ApiController]
    [Route("api/ngo/student-categories")]
    public class StudentCategoryController : ControllerBase
    {
        private readonly IStudentCategoryService _service;

        public StudentCategoryController(IStudentCategoryService service)
        {
            _service = service;
        }


        // -------- CREATE --------
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(StudentCategoryRequestDto dto)
        {
            dto.CreatedDate = DateTime.UtcNow;                            // always server-side
            dto.CreatedBy = JwtClaimHelper.LoginId(User);                 // or from claims

            var id = await _service.CreateAsync(dto);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Student category created successfully"
            });
        }


        // -------- UPDATE --------
        [HttpPut("update/{id:long}")]
        [Authorize]
        public async Task<IActionResult> Update(long id, [FromBody] StudentCategoryRequestDto dto)
        {
            dto.StudentCategoryId = id;
            dto.UpdatedDate = DateTime.UtcNow;                            // always server-side
            dto.UpdatedBy = JwtClaimHelper.LoginId(User);                 // or from claims

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
                Message = "Student category updated successfully",
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
                Message = "Student category deleted successfully",
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
        public async Task<IActionResult> GetByFilter(StudentCategoryFilterDto filter)
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
