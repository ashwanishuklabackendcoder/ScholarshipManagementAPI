using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Staff;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Common;

namespace ScholarshipManagementAPI.Controllers.Common
{
    [ApiController]
    [Route("api/common/staff")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _service;
        private readonly CurrentUserContextService _currentUser;

        public StaffController(IStaffService service, CurrentUserContextService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }



        // -------- CREATE --------
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(StaffRequestDto dto)
        {
            var currentUser = await _currentUser.GetCurrentUserAsync();
            var id = await _service.CreateAsync(dto,currentUser);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Staff created successfully"
            });
        }


        // -------- UPDATE --------
        [HttpPut("update/{id:long}")]
        [Authorize]
        public async Task<IActionResult> Update(long id, [FromBody] StaffRequestDto dto)
        {
            dto.StaffId = id;
            var currentUser = await _currentUser.GetCurrentUserAsync();
            var updated = await _service.UpdateAsync(dto, currentUser);

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
                Message = "Staff updated successfully",
                Result = updated,
            });
        }


        // -------- DELETE --------
        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(long id)
        {
            var currentUser = await _currentUser.GetCurrentUserAsync();
            var deleted = await _service.DeleteAsync(id,currentUser);

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
                Message = "User deleted successfully",
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
        public async Task<IActionResult> GetByFilter(StaffFilterDto filter)
        {
            var result = await _service.GetByFilterAsync(filter, await _currentUser.GetCurrentUserAsync());

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
