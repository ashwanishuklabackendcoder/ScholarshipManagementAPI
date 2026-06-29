using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.Faculties;
using ScholarshipManagementAPI.DTOs.University.MasterUniversity;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Implementation.University;
using ScholarshipManagementAPI.Services.Interface.University;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScholarshipManagementAPI.Controllers.University
{
    [ApiController]
    [Route("api/university/faculties")]
    public class FacultiesController : ControllerBase
    {
        private readonly IFacultiesService _service;

        public FacultiesController(IFacultiesService service)
        {
            _service = service;
        }

        // -------- CREATE --------
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(FacultyRequestDto dto)
        {
            dto.CreatedDate = DateTime.UtcNow;                            // always server-side
            dto.CreatedBy = JwtClaimHelper.LoginId(User);                 // or from claims

            var id = await _service.CreateAsync(dto);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Faculty created successfully"
            });
        }


        // -------- UPDATE --------
        [HttpPut("update/{id:long}")]
        [Authorize]
        public async Task<IActionResult> Update(long id, [FromBody] FacultyRequestDto dto)
        {
            dto.FacultyId = id;
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
                Message = "Faculty updated successfully",
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
                Message = "Faculty deleted successfully",
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
        public async Task<IActionResult> GetByFilter(FacultyFilterDto filter)
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



        // ---------------- GET FACULTY PROGRAMS DASHBOARD ----------------
        [HttpGet("faculty-programs/{universityId}")]
        public async Task<IActionResult> Dashboard(long universityId)
        {
            var data = await _service.GetFacultyProgramsDashboardAsync(universityId);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = data,
                Message = "Record found"
            });

        }

    }
}
