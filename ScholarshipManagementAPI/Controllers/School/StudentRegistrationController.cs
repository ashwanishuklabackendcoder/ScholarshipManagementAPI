using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentRegistration;
using ScholarshipManagementAPI.Services.Interface.School;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Controllers.School
{
    [ApiController]
    [Route("api/school/student-registration")]
    public class StudentRegistrationController : ControllerBase
    {
        private readonly IStudentRegistrationService _service;

        public StudentRegistrationController(IStudentRegistrationService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] StudentRegistrationRequestDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Student registration submitted successfully."
            });
        }

        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> Update(long id, [FromForm] StudentRegistrationRequestDto dto)
        {
            dto.StudentId = id;
            var updated = await _service.UpdateAsync(dto);

            if (!updated)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Record not found.",
                    Result = null
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Student registration updated successfully.",
                Result = updated
            });
        }

        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Record not found.",
                    Result = null
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Student registration deleted successfully.",
                Result = deleted
            });
        }

        [HttpGet("getById/{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Record not found.",
                    Result = null
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = data,
                Message = "Record found."
            });
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetByFilter(StudentRegistrationFilterDto filter)
        {
            var result = await _service.GetByFilterAsync(filter);

            return Ok(new ApiResponseDto
            {
                Success = result.Items.Count > 0,
                Result = result,
                Message = result.Items.Count == 0 ? "Data not found." : "Data fetched successfully."
            });
        }
    }
}
