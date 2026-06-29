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
    }
}
