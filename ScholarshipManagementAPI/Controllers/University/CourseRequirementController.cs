using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentRequirements;
using ScholarshipManagementAPI.DTOs.School.Students;
using ScholarshipManagementAPI.DTOs.University.CourseRequirement;
using ScholarshipManagementAPI.DTOs.University.MasterCourse;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.University;

namespace ScholarshipManagementAPI.Controllers.University
{
    [ApiController]
    [Route("api/university/master-course-requirement")]
    public class CourseRequirementController : ControllerBase
    {
        private readonly ICourseRequirementService _service;
        private readonly CurrentUserContextService _currentUser;

        public CourseRequirementController(ICourseRequirementService service, CurrentUserContextService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }


        // -------- CREATE --------
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(CourseRequirementRequestDto dto)
        {
            dto.CreatedDate = DateTime.UtcNow;                              // always server-side
            dto.CreatedBy = JwtClaimHelper.UserName(User).ToString();        // or from claims

            var id = await _service.CreateAsync(dto, await _currentUser.GetCurrentUserAsync());

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Course requirement created successfully"
            });
        }


        // -------- UPDATE --------
        [HttpPut("update/{id:long}")]
        [Authorize]
        public async Task<IActionResult> Update(long id, [FromBody] CourseRequirementRequestDto dto)
        {
            dto.ReqId = id;
            var updated = await _service.UpdateAsync(dto, await _currentUser.GetCurrentUserAsync());

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
                Message = "Course requirement updated successfully",
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
                Message = "Course requirement deleted successfully",
                Result = deleted
            });
        }


        // -------- GET BY ID --------
        [HttpGet("getById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(long id)
        {
            var data = await _service.GetByIdAsync(id, await _currentUser.GetCurrentUserAsync());

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
        public async Task<IActionResult> GetByFilter(CourseRequirementFilterDto filter)
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

        // -------- ENROLLMENT / SEARCH --------
        [HttpPost("enrollments/search")]
        [Authorize]
        public async Task<IActionResult> GetEnrollments(CourseRequirementFilterDto filter)
        {
            var result = await _service.GetEnrollmentsAsync(
                filter,
                await _currentUser.GetCurrentUserAsync()
            );

            return Ok(new ApiResponseDto
            {
                Success = result.Items.Count > 0,
                Result = result,
                Message = result.Items.Count == 0
                    ? "No enrollment data found"
                    : "Enrollment data fetched successfully"
            });
        }


        // -------- ENROLLED STUDENTS --------
        [HttpPost("enrollments/students/search/{reqId}")]
        [Authorize]
        public async Task<IActionResult> GetEnrolledStudentsByReqId(long reqId, StudentRequirementFilterDto filter)
        {
            var result = await _service.GetEnrolledStudentsAsync(reqId, filter, await _currentUser.GetCurrentUserAsync());

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = result.Items.Count == 0
                    ? "No students found"
                    : "Students fetched successfully"
            });
        }


        // -------- All ENROLLED STUDENTS --------
        [HttpPost("enrollments/students/search-all")]
        [Authorize]
        public async Task<IActionResult> GetAllEnrolledStudents(StudentRequirementFilterDto filter)
        {
            var result = await _service.GetEnrolledStudentsAsync(null , filter, await _currentUser.GetCurrentUserAsync());

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = result.Items.Count == 0
                    ? "No students found"
                    : "Students fetched successfully"
            });
        }



    }
}
