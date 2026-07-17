using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentProgramApplication;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.School;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Controllers.School
{
    [ApiController]
    [Route("api/school/student-program")]
    public class StudentProgramApplicationController : ControllerBase
    {
        private readonly IStudentProgramApplicationService _service;

        public StudentProgramApplicationController(IStudentProgramApplicationService service)
        {
            _service = service;
        }


        [HttpGet("candidate-programs/{studentId:long}")]
        [Authorize]
        public async Task<IActionResult> GetCandidatePrograms(long studentId)
        {
            var data = await _service.GetCandidateProgramsAsync(studentId);
            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = data,
                Message = "Candidate programs retrieved successfully."
            });
        }


        [HttpPost("apply/{studentId:long}")]
        [Authorize]
        public async Task<IActionResult> Apply(long studentId, [FromBody] ApplyRequestDto dto)
        {
            long userId = JwtClaimHelper.LoginId(User);
            var id = await _service.ApplyAsync(studentId, dto, userId);
            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Program application draft created successfully."
            });
        }


        [HttpDelete("cancel/{applicationId:long}")]
        [Authorize]
        public async Task<IActionResult> CancelApplication(long applicationId)
        {
            long userId = JwtClaimHelper.LoginId(User);
            var success = await _service.CancelApplicationAsync(applicationId, userId);
            return Ok(new ApiResponseDto
            {
                Success = success,
                Result = success,
                Message = "Application draft cancelled and deleted successfully."
            });
        }


        [HttpPost("submit/{applicationId:long}")]
        [Authorize]
        public async Task<IActionResult> SubmitApplication(long applicationId)
        {
            long userId = JwtClaimHelper.LoginId(User);
            var success = await _service.SubmitApplicationAsync(applicationId, userId);
            return Ok(new ApiResponseDto
            {
                Success = success,
                Result = success,
                Message = "Application submitted successfully."
            });
        }


        [HttpGet("getById/{applicationId:long}")]
        [Authorize]
        public async Task<IActionResult> GetApplication(long applicationId)
        {
            var data = await _service.GetApplicationAsync(applicationId);
            if (data == null)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Application not found.",
                    Result = null
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = data,
                Message = "Application retrieved successfully."
            });
        }


        [HttpPost("upload-document/{applicationId:long}")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDocument(long applicationId, [FromForm] UploadDocumentRequestDto request)
        {
            long userId = JwtClaimHelper.LoginId(User);

            var doc = await _service.UploadDocumentAsync(
                applicationId,
                request.ProgramDocumentId,
                request.DocumentTypeId,
                request.File,
                userId);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = doc,
                Message = "Document uploaded successfully."
            });
        }


        [HttpDelete("delete-document/{applicationId:long}/{documentId:long}")]
        [Authorize]
        public async Task<IActionResult> DeleteDocument(long applicationId, long documentId)
        {
            long userId = JwtClaimHelper.LoginId(User);
            var success = await _service.DeleteDocumentAsync(applicationId, documentId, userId);
            return Ok(new ApiResponseDto
            {
                Success = success,
                Result = success,
                Message = "Document deleted successfully."
            });
        }


        [HttpGet("documents/{applicationId:long}")]
        [Authorize]
        public async Task<IActionResult> GetDocuments(long applicationId)
        {
            var data = await _service.GetDocumentsAsync(applicationId);
            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = data,
                Message = "Documents retrieved successfully."
            });
        }


        [HttpGet("history/{studentId:long}")]
        [Authorize]
        public async Task<IActionResult> GetHistory(long studentId)
        {
            var data = await _service.GetHistoryAsync(studentId);
            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = data,
                Message = "Student history retrieved successfully."
            });
        }
   
    }
}
