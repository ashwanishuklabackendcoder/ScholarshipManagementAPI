using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRole;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface IUsersRoleService
    {
        Task<long> CreateAsync(UsersRoleRequestDto dto);
        Task<bool> UpdateAsync(UsersRoleRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<UsersRoleRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<UsersRoleRequestDto>> GetByFilterAsync(UsersRoleFilterDto filter);
    }
}
