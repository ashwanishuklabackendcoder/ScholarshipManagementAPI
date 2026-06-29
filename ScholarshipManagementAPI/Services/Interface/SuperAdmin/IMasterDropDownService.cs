using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperADmin.ZzMasterDropdown;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface IMasterDropDownService
    {
        Task<long> CreateAsync(MasterDropDownRequestDto dto);
        Task<bool> UpdateAsync(MasterDropDownRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<MasterDropDownRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<MasterDropDownRequestDto>> GetByFilterAsync(MasterDropDownFilterDto filter);


        //Task<List<MasterDropDownRequestDto>> GetByFilterAsync(MasterDropDownFilterDto filter);
        //Task<List<object>> GetAllAsync();

    }
}
