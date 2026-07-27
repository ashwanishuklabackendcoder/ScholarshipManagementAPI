using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.StudentCategory;

namespace ScholarshipManagementAPI.Services.Interface.Ngo
{
    public interface IStudentCategoryService
    {
        Task<long> CreateAsync(StudentCategoryRequestDto dto);

        Task<bool> UpdateAsync(StudentCategoryRequestDto dto);

        Task<bool> DeleteAsync(long id);

        Task<StudentCategoryRequestDto?> GetByIdAsync(long id);

        Task<PagedResultDto<StudentCategoryRequestDto>> GetByFilterAsync(StudentCategoryFilterDto filter);
    }
}
