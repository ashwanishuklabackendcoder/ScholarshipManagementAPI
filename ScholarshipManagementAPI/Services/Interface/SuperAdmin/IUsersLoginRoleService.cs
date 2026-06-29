using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLoginRole;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface IUsersLoginRoleService
    {
        Task<long> CreateAsync(UsersLoginRoleRequestDto dto);
        Task<bool> UpdateAsync(UsersLoginRoleRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<UsersLoginRoleRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<UsersLoginRoleRequestDto>> GetByFilterAsync(UsersLoginRoleFilterDto filter);


        Task<PagedResultDto<LoginRoleAssignmentDto>> GetRolesByLoginAsync(UsersLoginRoleFilterDto filter);

        Task BulkSaveRolesAsync(LoginRoleBulkSaveDto dto, string createdBy);

    }
}
