using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentRegistration;
using ScholarshipManagementAPI.Services.Interface.School;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Controllers.School
{
    [ApiController]
    [Route("students")]
    [Route("api/students")]
    public class StudentsApiController : ControllerBase
    {
        private readonly IStudentRegistrationService _service;

        public StudentsApiController(IStudentRegistrationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StudentRegistrationRequestDto dto)
        {
            dto.IsDraft = false; // "Student has no IsDraft"
            var id = await _service.CreateAsync(dto);
            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Student profile created successfully."
            });
        }

        [HttpGet("{id:long}")]
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

        [HttpPut("{id:long}")]
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
                Message = "Student profile updated successfully.",
                Result = updated
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? searchText, [FromQuery] string? gender, [FromQuery] string? schoolName, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var filter = new StudentRegistrationFilterDto
            {
                SearchText = searchText,
                Gender = gender,
                SchoolName = schoolName,
                PageNumber = pageNumber,
                PageSize = pageSize,
                IsActive = true
            };
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
