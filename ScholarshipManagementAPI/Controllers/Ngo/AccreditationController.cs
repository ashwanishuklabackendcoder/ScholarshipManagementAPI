using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo;
using ScholarshipManagementAPI.DTOs.Ngo.Accreditation;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Implementation.University;
using ScholarshipManagementAPI.Services.Interface.Ngo;
using ScholarshipManagementAPI.Services.Interface.University;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ScholarshipManagementAPI.Controllers.Ngo
{
    [ApiController]
    [Route("api/ngo/accreditation")]
    public class AccreditationController : ControllerBase
    {
        private readonly IAccreditationService _service;
        private readonly CurrentUserContextService _currentUser;

        public AccreditationController(IAccreditationService service, CurrentUserContextService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }



        [HttpPost("school")]
        [Authorize]
        public async Task<IActionResult> ApproveSchool(SchoolAccreditationDto dto)
        {
            var currentUser = await _currentUser.GetCurrentUserAsync();

            if (currentUser.StaffType != StaffType.Ngo)
                throw new CustomException("Only NGO can approve school");

            var result = await _service.AccreditSchoolAsync(dto);

            var message = dto.AccreditationStatus == AccreditationStatusEnum.Accredited
                ? "School accredited successfully."
                : "School accreditation rejected successfully.";

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = message,
                Result = result,
            });
        }


        [HttpPost("university")]
        [Authorize]
        public async Task<IActionResult> ApproveUniversity(UniversityAccreditationDto dto)
        {
            var currentUser = await _currentUser.GetCurrentUserAsync();

            if (currentUser.StaffType != StaffType.Ngo && currentUser.StaffType != StaffType.SuperAdmin)
                throw new CustomException("Only NGO can approve university");
            
            var result = await _service.AccreditUniversityAsync(dto);

            var message = dto.AccreditationStatus == AccreditationStatusEnum.Accredited
                ? "University accredited successfully."
                : "University accreditation rejected successfully.";

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = message,
                Result = result,
            });
        }



        [HttpPost("program")]
        public async Task<IActionResult> AccreditateProgram(ProgramAccreditationDto dto)
        {
            var currentUser = await _currentUser.GetCurrentUserAsync();

            if (currentUser.StaffType != StaffType.Ngo && currentUser.StaffType != StaffType.SuperAdmin)
                throw new CustomException("Only NGO can approve university");

            var result = await _service.AccreditProgramAsync(dto);

            var message = dto.AccreditationStatus == AccreditationStatusEnum.Accredited
                ? "Program accredited successfully."
                : "Program accreditation rejected successfully.";

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = message,
                Result = result,
            });

        }




    }
}
