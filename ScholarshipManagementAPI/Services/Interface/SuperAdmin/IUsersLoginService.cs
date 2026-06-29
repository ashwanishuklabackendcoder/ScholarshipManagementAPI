using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLogin;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface IUsersLoginService
    {
        Task<long> CreateAsync(UsersLoginRequestDto dto);
        Task<bool> UpdateAsync(UsersLoginRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<UsersLoginRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<UsersLoginRequestDto>> GetByFilterAsync(UsersLoginFilterDto filter);
    }
}
