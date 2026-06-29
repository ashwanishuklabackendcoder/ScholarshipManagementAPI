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


        [HttpPost("course-type")]
        [Authorize]
        public async Task<IActionResult> ApproveCourseType(ApprovalRequestDto dto)
        {
            // get logged-in user
            var currentUser = await _currentUser.GetCurrentUserAsync();

            // optional role guard (recommended)
            if (currentUser.StaffType != StaffType.Ngo && currentUser.StaffType != StaffType.SuperAdmin)
                throw new CustomException("Only NGO can approve course types");

            // approval status should be 1 (approved) or 2 (rejected)
            // entity id is the course type id here 
            var result = await _service.ApproveCourseTypeAsync(dto.EntityId, dto.ApprovalStatus, currentUser.LoginId);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Course type approval updated successfully",
                Result = result,
            });
        }


        [HttpPost("course")]
        [Authorize]
        public async Task<IActionResult> ApproveCourse(ApprovalRequestDto dto)
        {
            // get logged-in user
            var currentUser = await _currentUser.GetCurrentUserAsync();

            // optional role guard (recommended)
            if (currentUser.StaffType != StaffType.Ngo && currentUser.StaffType != StaffType.SuperAdmin)
                throw new CustomException("Only NGO can approve courses");

            // approval status should be 1 (approved) or 2 (rejected)
            // entity id is the course id here 
            var result = await _service.ApproveCourseAsync(dto.EntityId, dto.ApprovalStatus, currentUser.LoginId);


            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Course approval updated successfully",
                Result = result,
            });
        }
        
        
        [HttpPost("course-req")]
        [Authorize]
        public async Task<IActionResult> ApproveCourseRequirement(ApprovalRequestDto dto)
        {
            // get logged-in user
            var currentUser = await _currentUser.GetCurrentUserAsync();

            // optional role guard (recommended)
            if (currentUser.StaffType != StaffType.Ngo && currentUser.StaffType != StaffType.SuperAdmin)
                throw new CustomException("Only NGO can approve course requirements");

            // approval status should be 1 (approved) or 2 (rejected)
            // entity id is the course type id here 
            var result = await _service.ApproveCourseRequirementAsync(dto.EntityId, dto.ApprovalStatus, currentUser.LoginId);


            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Course requiremnet approved successfully",
                Result = result,
            });
        }
       
        
        [HttpPost("school")]
        [Authorize]
        public async Task<IActionResult> ApproveSchool(ApprovalRequestDto dto)
        {
            // get logged-in user
            var currentUser = await _currentUser.GetCurrentUserAsync();

            // optional role guard (recommended)
            if (currentUser.StaffType != StaffType.Ngo && currentUser.StaffType != StaffType.SuperAdmin)
                throw new CustomException("Only NGO can approve school");

            // approval status should be 1 (approved) or 2 (rejected)
            // entity id is the school id here 
            var result = await _service.ApproveSchoolAsync(dto.EntityId, dto.ApprovalStatus, currentUser.LoginId);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "School approval updated successfully",
                Result = result,
            });
        }


        [HttpPost("university")]
        [Authorize]
        public async Task<IActionResult> ApproveUniversity(ApprovalRequestDto dto)
        {
            // get logged-in user
            var currentUser = await _currentUser.GetCurrentUserAsync();

            // optional role guard (recommended)
            if (currentUser.StaffType != StaffType.Ngo && currentUser.StaffType != StaffType.SuperAdmin)
                throw new CustomException("Only NGO can approve university");

            // approval status should be 1 (approved) or 2 (rejected)
            // entity id is the school id here 
            // var result = await _service.ApproveUniversityAsync(dto.EntityId, dto.ApprovalStatus, currentUser.LoginId);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "University approval updated successfully",
                //Result = result,
            });
        }



        [HttpPost("program")]
        public async Task<IActionResult> AccreditateProgram(ProgramAccreditationDto dto)
        {

            var currentUser = await _currentUser.GetCurrentUserAsync();
            dto.UpdatedBy = currentUser.LoginId;
            var result = await _service.AccreditateProgram(dto);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Program updated successfully",
                Result = result,
            });

        }




    }
}
