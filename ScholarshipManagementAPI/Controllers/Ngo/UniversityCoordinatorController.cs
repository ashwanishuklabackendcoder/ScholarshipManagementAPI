using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.Administration.UniversityCoordinators;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Ngo;

namespace ScholarshipManagementAPI.Controllers.Ngo
{
    [ApiController]
    [Route("api/ngo/university-coordinators")]
    public class UniversityCoordinatorController : ControllerBase
    {
        private readonly IUniversityCoordinatorService _service;
        private readonly CurrentUserContextService _currentUserContext;

        public UniversityCoordinatorController(
            IUniversityCoordinatorService service, 
            CurrentUserContextService currentUserContext)
        {
            _service = service;
            _currentUserContext = currentUserContext;
        }


        // ---------------- CREATE ----------------
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] UniversityCoordinatorRequestDto dto)
        {
            var currentUser = await _currentUserContext.GetCurrentUserAsync();
            var id = await _service.CreateAsync(dto, currentUser);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "University coordinator created successfully."
            });
        }



        // ---------------- UPDATE ----------------
        [HttpPut("update/{staffId:long}")]
        [Authorize]
        public async Task<IActionResult> Update(long staffId, [FromBody] UniversityCoordinatorRequestDto dto)
        {
            dto.StaffId = staffId;
            var currentUser = await _currentUserContext.GetCurrentUserAsync();
            var updated = await _service.UpdateAsync(dto, currentUser);

            if (!updated)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Record not found."
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = updated,
                Message = "University coordinator updated successfully."
            });
        }



        // ---------------- DELETE ----------------
        [HttpDelete("delete/{staffId:long}")]
        [Authorize]
        public async Task<IActionResult> Delete(long staffId)
        {
            var currentUser = await _currentUserContext.GetCurrentUserAsync();
            var deleted = await _service.DeleteAsync(staffId, currentUser);

            if (!deleted)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Record not found."
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = deleted,
                Message = "University coordinator deleted successfully."
            });
        }



        // ---------------- GET BY ID ----------------
        [HttpGet("getById/{staffId:long}")]
        [Authorize]
        public async Task<IActionResult> GetById(long staffId)
        {
            var data = await _service.GetByIdAsync(staffId);
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
                Message = "Record fetched successfully."
            });
        }



        // ---------------- SEARCH ----------------
        [HttpPost("search")]
        [Authorize]
        public async Task<IActionResult> Search([FromBody] UniversityCoordinatorFilterDto filter)
        {
            var result = await _service.GetByFilterAsync(filter);

            return Ok(new ApiResponseDto
            {
                Success = result.Items.Any(),
                Result = result,
                Message = result.Items.Any()
                    ? "Data fetched successfully."
                    : "Data not found."
            });
        }


    }
}


