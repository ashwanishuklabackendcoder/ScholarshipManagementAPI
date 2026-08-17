using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.MasterSchool;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersMenu;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.School;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Controllers.School
{
    [ApiController]
    [Route("api/school/master-school")]
    public class MasterSchoolController : ControllerBase
    {
        private readonly IMasterSchoolService _service;

        public MasterSchoolController(IMasterSchoolService service)
        {
            _service = service;
        }


        // -------- CREATE --------
        [HttpPost("create")]
        public async Task<IActionResult> Create(MasterSchoolRequestDto dto)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                dto.CreatedBy = JwtClaimHelper.LoginId(User);
            }

            var id = await _service.CreateAsync(dto);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "School created successfully"
            });
        }


        // -------- UPDATE --------
        [HttpPut("update/{id:long}")]
        [Authorize]
        public async Task<IActionResult> Update(long id, [FromBody] MasterSchoolRequestDto dto)
        {
            dto.SchoolId = id;
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
                Message = "School updated successfully",
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
                Message = "School deleted successfully",
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
        public async Task<IActionResult> GetByFilter(MasterSchoolFilterDto filter)
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



        // ---------------- GET SCHOOLS BY COUNTRY IDS ----------------
        [HttpPost("getSchoolsByCountryIds")]
        [Authorize]
        public async Task<IActionResult> GetSchoolsByCountryIds([FromBody] SchoolByCountryRequestDto dto)
        {
            var result = await _service.GetSchoolsByCountryIdsAsync(dto.CountryIds);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = "Schools fetched successfully."
            });
        }




    }
}
