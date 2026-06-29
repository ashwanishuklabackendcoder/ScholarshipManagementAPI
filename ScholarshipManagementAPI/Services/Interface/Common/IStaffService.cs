using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.HrStaff;
using ScholarshipManagementAPI.DTOs.Common.Response;

namespace ScholarshipManagementAPI.Services.Interface.Common
{
    public interface IStaffService
    {
        Task<long> CreateAsync(StaffRequestDto dto);
        Task<bool> UpdateAsync(StaffRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<StaffRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<StaffRequestDto>> GetByFilterAsync(StaffFilterDto filter, LoggedInUserDto currentUser);
    }
}
