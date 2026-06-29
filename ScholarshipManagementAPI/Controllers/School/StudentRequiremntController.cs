using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentRequirements;
using ScholarshipManagementAPI.DTOs.School.Students;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Implementation.School;
using ScholarshipManagementAPI.Services.Interface.School;

namespace ScholarshipManagementAPI.Controllers.School
{
    [ApiController]
    [Route("api/school/student-req")]
    public class StudentRequiremntController : ControllerBase
    {
        private readonly IStudentRequirementService _service;
        private readonly CurrentUserContextService _currentUser;

        public StudentRequiremntController(IStudentRequirementService service, CurrentUserContextService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }


        // -------- CREATE --------
        [HttpPost("create")]
        public async Task<IActionResult> CreateStudentRequirementMap(StudentRequirementMappingDto dto)
        {
            dto.CreatedBy = JwtClaimHelper.UserName(User);
            dto.CreatedDate = DateTime.UtcNow;

            var id = await _service.CreateStudentRequirementMapAsync(dto);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Student requirement created successfully"
            });
        }


        // -------- UPDATE --------
        [HttpPut("update/{id:long}")]
        [Authorize]
        public async Task<IActionResult> UpdateStudentRequirementMap(long id, [FromBody] StudentRequirementMappingDto dto)
        {
            dto.StudentReqID = id;
            var updated = await _service.UpdateStudentRequirementMapAsync(dto);

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
                Message = "Student requirement updated successfully",
                Result = updated,
            });
        }


        // -------- UPDATE --------
        [HttpPut("update-by-university/{id:long}")]
        [Authorize]
        public async Task<IActionResult> UpdateStudentRequirementMapByUniversityAsync(long id, [FromBody] StudentRequirementRequestDto dto)
        {
            dto.StudentReqID = id;
            var updated = await _service.UpdateStudentRequirementMapByUniversityAsync(dto, await _currentUser.GetCurrentUserAsync());

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
                Message = "Student status updated successfully",
                Result = updated,
            });
        }


        // -------- UPDATE --------
        [HttpPut("update-by-ngo/{id:long}")]
        [Authorize]
        public async Task<IActionResult> UpdateStudentRequirementMapByNgoAsync(long id, [FromBody] StudentRequirementRequestDto dto)
        {
            dto.StudentReqID = id;
            var updated = await _service.UpdateStudentRequirementMapByNgoAsync(dto, await _currentUser.GetCurrentUserAsync());

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
                Message = "Student status updated successfully",
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
                Message = "Student requirement deleted successfully",
                Result = deleted
            });
        }


        // -------- GET BY ID --------
        [HttpGet("getById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(long id)
        {
            var data = await _service.GetByIdAsync(id);

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
        public async Task<IActionResult> GetByFilter(StudentRequirementFilterDto filter)
        {
            var result = await _service.GetByFilterAsync(filter);

            return Ok(new ApiResponseDto
            {
                Success = result.Items.Count == 0 ? false : true,
                Result = result,
                Message = result.Items.Count == 0
                    ? "Data not found"
                    : "Data fetched successfully"
            });
        }





        //[HttpPost("upload/{studentReqId}/{masterDocId}")]
        //public async Task<IActionResult> UploadDocument(long studentReqId, long masterDocId, IFormFile file)
        //{
        //    var result = await _service.UploadAsync(
        //        studentReqId,
        //        masterDocId,
        //        file);

        //    //return Ok(result);


        //    return Ok(new ApiResponseDto
        //    {
        //        Success = true,
        //        Result = result,
        //        Message = "Document uploaded successfully"
        //    });
        //}

        //[HttpGet("documents/{studentReqId}")]
        //public async Task<IActionResult> GetDocuments(long studentReqId)
        //{
        //    var result = await _service
        //        .GetDocumentStatusAsync(studentReqId);

        //    //return Ok(result);


        //    return Ok(new ApiResponseDto
        //    {
        //        Success = true,
        //        Result = result,
        //        Message = "Document fetched successfully"
        //    });

        //}



        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadDocumentRequestDto dto)
        {
            var result = await _service.UploadAsync(dto);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = "Document uploaded successfully"
            });
        }



        [HttpPost("document-status")]
        public async Task<IActionResult> GetDocumentStatus(DocumentStatusRequestDto request)
        {
            var result = await _service.GetDocumentStatusAsync(request);
            //return Ok(result);



            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = "Document fetched successfully"
            });
        }


    }
}
