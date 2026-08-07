using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.Students;

namespace ScholarshipManagementAPI.Services.Interface.School
{
    public interface IStudentService
    {
        Task<long> CreateAsync(StudentRequestDto dto, LoggedInUserDto currentUser);
        Task<bool> UpdateAsync(StudentRequestDto dto, LoggedInUserDto currentUser);
        Task<bool> DeleteAsync(long id, LoggedInUserDto currentUser);

        Task<StudentRequestDto?> GetByIdAsync(long id, LoggedInUserDto currentUser);
        Task<PagedResultDto<StudentRequestDto>> GetByFilterAsync(StudentFilterDto filter, LoggedInUserDto currentUser);


        Task<string> UploadProfilePhotoAsync(long studentId, IFormFile file, LoggedInUserDto currentUser);

        Task<bool> DeleteProfilePhotoAsync(long studentId, LoggedInUserDto currentUser);

        Task<string> UploadRecommendationLetterAsync(long studentId, IFormFile file, LoggedInUserDto currentUser);
        Task<bool> DeleteRecommendationLetterAsync(long studentId, LoggedInUserDto currentUser);
    
    }
}
