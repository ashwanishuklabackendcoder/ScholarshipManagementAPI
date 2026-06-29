using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.CountrySchoolsSummary;
using ScholarshipManagementAPI.DTOs.SuperAdmin.MasterCountry;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface IMasterCountryService
    {
        Task<long> CreateAsync(MasterCountryRequestDto dto);
        Task<bool> UpdateAsync(MasterCountryRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<MasterCountryRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<MasterCountryRequestDto>> GetByFilterAsync(MasterCountryFilterDto filter);


        Task<PagedResultDto<CountrySchoolCountDto>> GetCountryWiseSchoolCountAsync(MasterCountryFilterDto filter);

    }
}
