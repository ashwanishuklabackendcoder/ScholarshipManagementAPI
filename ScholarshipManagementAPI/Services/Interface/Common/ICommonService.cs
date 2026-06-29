using ScholarshipManagementAPI.DTOs.Common.HrStaff;
using ScholarshipManagementAPI.DTOs.Common.Menu;
using ScholarshipManagementAPI.DTOs.Common.Settings;
using ScholarshipManagementAPI.DTOs.SuperADmin.ZzMasterDropdown;

namespace ScholarshipManagementAPI.Services.Interface.Common
{
    public interface ICommonService
    {
        Task<List<UsersModuleDto>> GetAllUsersModule();

        Task<List<LoadMenuDto>> LoadMenusByRoleAsync(long roleId);

        // dashboard service
        Task<DashboardDto> GetDashboardAsync();


        // profile upload service
        Task<string> UploadUserProfileImageAsync(int userId, IFormFile file);

        string? GetProfileImageUrl(string? fileKey);


        // media upload service
        Task<string> UploadUserDocumentAsync(int userId, IFormFile file);

        string? GetDocumentUrl(string? fileKey);





    }
}
