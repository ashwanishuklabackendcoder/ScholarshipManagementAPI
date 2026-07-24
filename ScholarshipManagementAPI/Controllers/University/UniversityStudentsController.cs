using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.Students;
using ScholarshipManagementAPI.DTOs.University.Students;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.School;
using ScholarshipManagementAPI.Services.Interface.University;

namespace ScholarshipManagementAPI.Controllers.University
{
    [ApiController]
    [Route("api/university/students")]
    public class UniversityStudentsController : ControllerBase
    {

        private readonly IUniversityStudentService _service;
        private readonly CurrentUserContextService _currentUser;

        public UniversityStudentsController(IUniversityStudentService service , CurrentUserContextService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }



        // -------- GET BY ID --------
        [HttpGet("getById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(long id)
        {
            long loginId = JwtClaimHelper.LoginId(User);
            var currentUser = await _currentUser.GetCurrentUserAsync();

            var data = await _service.GetByIdAsync(id, loginId, currentUser);

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
        public async Task<IActionResult> GetByFilter(UniversityStudentFilterDto filter)
        {
            var currentUser = await _currentUser.GetCurrentUserAsync();
            var result = await _service.GetByFilterAsync(filter, currentUser);

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
