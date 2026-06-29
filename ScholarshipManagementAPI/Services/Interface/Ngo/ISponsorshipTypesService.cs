using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.SponsorshipTypes;

namespace ScholarshipManagementAPI.Services.Interface.Ngo
{
    public interface ISponsorshipTypesService
    {
        Task<long> CreateAsync(SponsorshipTypeRequestDto dto);

        Task<bool> UpdateAsync(SponsorshipTypeRequestDto dto);

        Task<bool> DeleteAsync(long id);

        Task<SponsorshipTypeRequestDto?> GetByIdAsync(long id);

        Task<PagedResultDto<SponsorshipTypeRequestDto>>GetByFilterAsync(SponsorshipTypeFilterDto filter);
    }
}
