using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersMenu;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface IUsersMenuService
    {
        Task<long> CreateAsync(UsersMenuRequestdto dto);
        Task<bool> UpdateAsync(UsersMenuRequestdto dto);
        Task<bool> DeleteAsync(long id);

        Task<UsersMenuRequestdto?> GetByIdAsync(long id);
        Task<PagedResultDto<UsersMenuRequestdto>> GetByFilterAsync(UsersMenuFilterDto filter);
    }
}
