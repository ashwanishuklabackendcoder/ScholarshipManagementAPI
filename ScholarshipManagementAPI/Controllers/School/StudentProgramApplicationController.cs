using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentProgramApplication;
using ScholarshipManagementAPI.Services.Interface.School;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Controllers.School
{
    [ApiController]
    public class StudentProgramApplicationController : ControllerBase
    {
        private readonly IStudentProgramApplicationService _service;

        public StudentProgramApplicationController(IStudentProgramApplicationService service)
        {
            _service = service;
        }

        // GET /students/{studentId}/candidate-programs
        [HttpGet("students/{studentId:long}/candidate-programs")]
        [HttpGet("api/students/{studentId:long}/candidate-programs")]
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

        // POST /students/{studentId}/applications/apply
        [HttpPost("students/{studentId:long}/applications/apply")]
        [HttpPost("api/students/{studentId:long}/applications/apply")]
        public async Task<IActionResult> Apply(long studentId, [FromBody] ApplyRequestDto dto)
        {
            long userId = 2; // Default system/coordinator user id for audit trail
            var id = await _service.ApplyAsync(studentId, dto, userId);
            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Program application draft created successfully."
            });
        }

        // DELETE /applications/{applicationId}
        [HttpDelete("applications/{applicationId:long}")]
        [HttpDelete("api/applications/{applicationId:long}")]
        public async Task<IActionResult> CancelApplication(long applicationId)
        {
            long userId = 2;
            var success = await _service.CancelApplicationAsync(applicationId, userId);
            return Ok(new ApiResponseDto
            {
                Success = success,
                Result = success,
                Message = "Application draft cancelled and deleted successfully."
            });
        }

        // POST /applications/{applicationId}/submit
        [HttpPost("applications/{applicationId:long}/submit")]
        [HttpPost("api/applications/{applicationId:long}/submit")]
        public async Task<IActionResult> SubmitApplication(long applicationId)
        {
            long userId = 2;
            var success = await _service.SubmitApplicationAsync(applicationId, userId);
            return Ok(new ApiResponseDto
            {
                Success = success,
                Result = success,
                Message = "Application submitted successfully."
            });
        }

        // GET /applications/{applicationId}
        [HttpGet("applications/{applicationId:long}")]
        [HttpGet("api/applications/{applicationId:long}")]
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

        // POST /applications/{applicationId}/documents
        [HttpPost("applications/{applicationId:long}/documents")]
        [HttpPost("api/applications/{applicationId:long}/documents")]
        public async Task<IActionResult> UploadDocument(long applicationId, [FromForm] long programDocumentId, [FromForm] long documentTypeId, IFormFile file)
        {
            long userId = 2;
            var doc = await _service.UploadDocumentAsync(applicationId, programDocumentId, documentTypeId, file, userId);
            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = doc,
                Message = "Document uploaded successfully."
            });
        }

        // DELETE /applications/{applicationId}/documents/{documentId}
        [HttpDelete("applications/{applicationId:long}/documents/{documentId:long}")]
        [HttpDelete("api/applications/{applicationId:long}/documents/{documentId:long}")]
        public async Task<IActionResult> DeleteDocument(long applicationId, long documentId)
        {
            long userId = 2;
            var success = await _service.DeleteDocumentAsync(applicationId, documentId, userId);
            return Ok(new ApiResponseDto
            {
                Success = success,
                Result = success,
                Message = "Document deleted successfully."
            });
        }

        // GET /applications/{applicationId}/documents
        [HttpGet("applications/{applicationId:long}/documents")]
        [HttpGet("api/applications/{applicationId:long}/documents")]
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

        // GET /students/{studentId}/history
        [HttpGet("students/{studentId:long}/history")]
        [HttpGet("api/students/{studentId:long}/history")]
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
