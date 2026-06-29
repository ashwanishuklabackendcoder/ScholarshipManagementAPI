using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.University;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Controllers.University
{
    [ApiController]
    public class UniversityRegistrationController : ControllerBase
    {
        private readonly IUniversityRegistrationService _service;
        private readonly CurrentUserContextService _currentUser;

        public UniversityRegistrationController(IUniversityRegistrationService service, CurrentUserContextService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }

        // -------- PUBLIC SELF-REGISTRATION --------
        [HttpPost("api/university/register")]
        public async Task<IActionResult> Register([FromBody] UniversityRegistrationDto dto)
        {
            var id = await _service.RegisterAsync(dto);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "University self-registration submitted successfully."
            });
        }

        // -------- SUPER ADMIN VIEW PENDING --------
        [HttpGet("api/superadmin/university-registrations")]
        [Authorize]
        public async Task<IActionResult> GetPending([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var data = await _service.GetRegistrationsByStatusAsync(0, pageNumber, pageSize); // 0 = Pending

            return Ok(new ApiResponseDto
            {
                Success = data.Items.Count > 0,
                Result = data,
                Message = data.Items.Count > 0 ? "Pending university registrations retrieved." : "No pending registrations found."
            });
        }

        // -------- SUPER ADMIN APPROVE / REJECT --------
        [HttpPost("api/superadmin/university-registrations/{id:long}/approve")]
        [Authorize]
        public async Task<IActionResult> Approve(long id, [FromQuery] int status)
        {
            long approvedByUserId = 2;
            try
            {
                var user = await _currentUser.GetCurrentUserAsync();
                if (user != null) approvedByUserId = user.LoginId;
            }
            catch { }

            var success = await _service.ApproveRegistrationAsync(id, status, approvedByUserId);

            if (!success)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Registration record not found or already processed."
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = status == 1 ? "University approved and accredited." : "University registration rejected."
            });
        }
    }
}
