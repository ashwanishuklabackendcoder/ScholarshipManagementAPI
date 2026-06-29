using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLoginLog;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface IUsersLoginLogService
    {
        Task<long> CreateAsync(UsersLoginLogRequestDto dto);
        Task<bool> UpdateAsync(UsersLoginLogRequestDto dto);

        //Task<bool> DeleteAsync(long id);

        Task<UsersLoginLogRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<UsersLoginLogRequestDto>> GetByFilterAsync(UsersLoginLogFilterDto filter, LoggedInUserDto currentUser);
    }
}
