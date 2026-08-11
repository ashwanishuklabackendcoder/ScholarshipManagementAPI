using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRolePermission;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface IUsersRolePermissionService
    {
        Task<long> CreateAsync(UsersRolePermissionRequestDto dto);
        Task<bool> UpdateAsync(UsersRolePermissionRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<UsersRolePermissionRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<UsersRolePermissionRequestDto>> GetByFilterAsync(UsersRolePermissionFilterDto filter);


        Task<PagedResultDto<UsersRolePermissionDto>> GetRolePermissionsAsync(UsersRolePermissionFilterDto filter);


        Task BulkSaveRolePermissionsAsync(UsersRolePermissionBulkSaveDto dto, long createdBy);

    }
}
