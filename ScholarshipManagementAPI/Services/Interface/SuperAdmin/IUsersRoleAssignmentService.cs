using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRoleAssignment;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface IUsersRoleAssignmentService
    {
        Task<long> CreateAsync(UsersRoleAssignmentRequestDto dto);
        Task<bool> UpdateAsync(UsersRoleAssignmentRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<UsersRoleAssignmentRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<UsersRoleAssignmentRequestDto>> GetByFilterAsync(UsersRoleAssignmentFilterDto filter);


        Task<PagedResultDto<UsersRoleAssignmentDto>> GetRolesByLoginAsync(UsersRoleAssignmentFilterDto filter);

        Task BulkSaveRolesAsync(UsersRoleAssignmentSaveDto dto, long createdBy);

    }
}
