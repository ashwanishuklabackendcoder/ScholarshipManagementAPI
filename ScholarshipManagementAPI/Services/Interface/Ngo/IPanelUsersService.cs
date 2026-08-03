using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.Administration.PanelUsers;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Interface.Ngo
{
    public interface IPanelUsersService
    {
        Task<long> CreateAsync(PanelUserRequestDto dto, LoggedInUserDto currentUser);

        Task<bool> UpdateAsync(PanelUserRequestDto dto, LoggedInUserDto currentUser);

        Task<bool> DeleteAsync(long staffId, LoggedInUserDto currentUser);


        Task<PanelUserRequestDto> GetByIdAsync(long staffId);

        Task<PagedResultDto<PanelUserRequestDto>> GetByFilterAsync(PanelUserFilterDto filter);

    }
}
