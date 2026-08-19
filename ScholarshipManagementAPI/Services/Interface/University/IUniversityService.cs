using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University;
using ScholarshipManagementAPI.DTOs.University.MasterUniversity;

namespace ScholarshipManagementAPI.Services.Interface.University
{
    public interface IUniversityService
    {
        Task<long> CreateAsync(UniversityRequestDto dto);
        Task<bool> UpdateAsync(UniversityRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<UniversityRequestDto?> GetByIdAsync(long id, LoggedInUserDto currentUser);
        Task<PagedResultDto<UniversityRequestDto>> GetByFilterAsync(UniversityFilterDto filter, LoggedInUserDto currentUser);
    }
}
