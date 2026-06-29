using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.AdminEmailTemplate;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface IAdminEmailTemplateService
    {
        Task<long> CreateAsync(AdminEmailTemplateRequestDto dto);
        Task<bool> UpdateAsync(AdminEmailTemplateRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<AdminEmailTemplateRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<AdminEmailTemplateRequestDto>> GetByFilterAsync(AdminEmailTemplateFilterDto filter);
    }
}
