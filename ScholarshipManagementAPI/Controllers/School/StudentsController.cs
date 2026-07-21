using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.Students;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.School;

namespace ScholarshipManagementAPI.Controllers.School
{
    [ApiController]
    [Route("api/school/student")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentsController(IStudentService service)
        {
            _service = service;
        }

        // -------- CREATE --------
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] StudentRequestDto dto)
        {
            //, IFormFile? RecommendationLetterFile
            //dto.RecommendationLetterFile = RecommendationLetterFile;
            dto.CreatedBy = JwtClaimHelper.LoginId(User);
            dto.CreatedDate = DateTime.UtcNow;

            var id = await _service.CreateAsync(dto);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "Student created successfully"
            });
        }


        // -------- UPDATE --------
        [HttpPut("update/{id:long}")]
        [Authorize]
        public async Task<IActionResult> Update(long id, [FromForm] StudentRequestDto dto)
        {
            //, IFormFile? RecommendationLetterFile
            //dto.RecommendationLetterFile = RecommendationLetterFile;

            dto.StudentId = id;
            var updated = await _service.UpdateAsync(dto);

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
                Message = "Student updated successfully",
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
                Message = "Student deleted successfully",
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
        //[Authorize]
        public async Task<IActionResult> GetByFilter(StudentFilterDto filter)
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





        [HttpPost("upload-profile-photo/{studentId:long}")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProfilePhoto(long studentId, [FromForm] UploadFileRequestDto request)
        {
            long userId = JwtClaimHelper.LoginId(User);

            var result = await _service.UploadProfilePhotoAsync(
                studentId,
                request.File,
                userId);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Profile photo uploaded successfully.",
                Result = result
            });
        }


        [HttpDelete("delete-profile-photo/{studentId:long}")]
        [Authorize]
        public async Task<IActionResult> DeleteProfilePhoto(long studentId)
        {
            long userId = JwtClaimHelper.LoginId(User);

            await _service.DeleteProfilePhotoAsync(
                studentId,
                userId);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Profile photo deleted successfully."
            });
        }

        [HttpPost("upload-recommendation-letter/{studentId:long}")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadRecommendationLetter(long studentId, [FromForm] UploadFileRequestDto request)
        {
            long userId = JwtClaimHelper.LoginId(User);

            var result = await _service.UploadRecommendationLetterAsync(
                studentId,
                request.File,
                userId);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Recommendation letter uploaded successfully.",
                Result = result
            });
        }

        [HttpDelete("delete-recommendation-letter/{studentId:long}")]
        [Authorize]
        public async Task<IActionResult> DeleteRecommendationLetter(long studentId)
        {
            long userId = JwtClaimHelper.LoginId(User);

            await _service.DeleteRecommendationLetterAsync(
                studentId,
                userId);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Recommendation letter deleted successfully."
            });
        }



    }
}
