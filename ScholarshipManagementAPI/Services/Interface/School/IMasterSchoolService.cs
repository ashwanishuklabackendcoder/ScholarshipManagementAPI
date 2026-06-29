using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.MasterSchool;

namespace ScholarshipManagementAPI.Services.Interface.School
{
    public interface IMasterSchoolService
    {
        Task<long> CreateAsync(MasterSchoolRequestDto dto);
        Task<bool> UpdateAsync(MasterSchoolRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<MasterSchoolRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<MasterSchoolRequestDto>> GetByFilterAsync(MasterSchoolFilterDto filter);
    }
}
