using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.Students;

namespace ScholarshipManagementAPI.Services.Interface.School
{
    public interface IStudentService
    {
        Task<long> CreateAsync(StudentRequestDto dto);
        Task<bool> UpdateAsync(StudentRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<StudentRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<StudentRequestDto>> GetByFilterAsync(StudentFilterDto filter);


        Task<string> UploadProfilePhotoAsync(long studentId, IFormFile file, long userId);

        Task<bool> DeleteProfilePhotoAsync(long studentId, long userId);

        Task<string> UploadRecommendationLetterAsync(long studentId, IFormFile file, long userId);

        Task<bool> DeleteRecommendationLetterAsync(long studentId, long userId);
    
    }
}
