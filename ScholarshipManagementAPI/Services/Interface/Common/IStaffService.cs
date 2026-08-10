using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Staff;
using ScholarshipManagementAPI.DTOs.Common.Response;

namespace ScholarshipManagementAPI.Services.Interface.Common
{
    public interface IStaffService
    {
        Task<long> CreateAsync(StaffRequestDto dto, LoggedInUserDto currentUser);
        Task<bool> UpdateAsync(StaffRequestDto dto, LoggedInUserDto currentUser);
        Task<bool> DeleteAsync(long id, LoggedInUserDto currentUser);

        Task<StaffRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<StaffRequestDto>> GetByFilterAsync(StaffFilterDto filter, LoggedInUserDto currentUser);
    }
}
