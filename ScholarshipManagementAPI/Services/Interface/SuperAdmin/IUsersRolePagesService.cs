using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRole;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRolePage;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface IUsersRolePagesService
    {
        Task<long> CreateAsync(UsersRolePageRequestDto dto);
        Task<bool> UpdateAsync(UsersRolePageRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<UsersRolePageRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<UsersRolePageRequestDto>> GetByFilterAsync(UsersRolePageFilterDto filter);


        Task<PagedResultDto<RolePagePermissionDto>> GetRolePermissionsAsync(UsersRolePageFilterDto filter);


        Task BulkSaveRolePermissionsAsync(RolePermissionBulkSaveDto dto, string createdBy);

    }
}
