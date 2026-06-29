//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using ScholarshipManagementAPI.DTOs.Common.Response;
//using ScholarshipManagementAPI.DTOs.University.MasterCourse;
//using ScholarshipManagementAPI.DTOs.University.MasterCourseType;
//using ScholarshipManagementAPI.Helper.Utilities;
//using ScholarshipManagementAPI.Services.Interface.University;

//namespace ScholarshipManagementAPI.Controllers.University
//{
//    [ApiController]
//    [Route("api/university/master-course")]
//    public class CourseController : ControllerBase
//    {
//        private readonly ICourseService _service; 
//        private readonly CurrentUserContextService _currentUser;

//        public CourseController(ICourseService service, CurrentUserContextService currentUser)
//        {
//            _service = service;
//            _currentUser = currentUser;
//        }

//        // -------- CREATE --------
//        [HttpPost("create")]
//        [Authorize]
//        public async Task<IActionResult> Create(MasterCourseRequestDto dto)
//        {
//            dto.CreatedDate = DateTime.UtcNow;                              // always server-side
//            dto.CreatedBy = JwtClaimHelper.UserName(User).ToString();        // or from claims

//            var id = await _service.CreateAsync(dto);

//            return Ok(new ApiResponseDto
//            {
//                Success = true,
//                Result = id,
//                Message = "Course created successfully"
//            });
//        }


//        // -------- UPDATE --------
//        [HttpPut("update/{id:long}")]
//        [Authorize]
//        public async Task<IActionResult> Update(long id, [FromBody] MasterCourseRequestDto dto)
//        {
//            dto.UniversityId = id;
//            var updated = await _service.UpdateAsync(dto);

//            if (!updated)
//            {
//                return NotFound(new ApiResponseDto
//                {
//                    Success = false,
//                    Message = "Record not found",
//                    Result = null,
//                });
//            }

//            return Ok(new ApiResponseDto
//            {
//                Success = true,
//                Message = "Course updated successfully",
//                Result = updated,
//            });
//        }

//        // -------- DELETE --------
//        [HttpDelete("delete/{id}")]
//        [Authorize]
//        public async Task<IActionResult> Delete(long id)
//        {
//            var deleted = await _service.DeleteAsync(id);

//            if (!deleted)
//            {
//                return NotFound(new ApiResponseDto
//                {
//                    Success = false,
//                    Message = "Record not found",
//                    Result = null
//                });
//            }

//            return Ok(new ApiResponseDto
//            {
//                Success = true,
//                Message = "Course deleted successfully",
//                Result = deleted
//            });
//        }


//        // -------- GET BY ID --------
//        [HttpGet("getById/{id}")]
//        [Authorize]
//        public async Task<IActionResult> GetById(long id)
//        {
//            var data = await _service.GetByIdAsync(id);

//            if (data == null)
//            {
//                return NotFound(new ApiResponseDto
//                {
//                    Success = false,
//                    Message = "Record not found",
//                    Result = null
//                });
//            }

//            return Ok(new ApiResponseDto
//            {
//                Success = true,
//                Result = data,
//                Message = "Record found"
//            });
//        }


//        // -------- FILTER / GET ALL --------
//        [HttpPost("search")]
//        [Authorize]
//        public async Task<IActionResult> GetByFilter(MasterCourseFilterDto filter)
//        {
//            var result = await _service.GetByFilterAsync(filter, await _currentUser.GetCurrentUserAsync());

//            return Ok(new ApiResponseDto
//            {
//                Success = result.Items.Count == 0 ? false : true,
//                Result = result,
//                Message = result.Items.Count == 0
//                    ? "Data not found"
//                    : "Data fetched successfully"
//            });
//        }

//    }
//}
