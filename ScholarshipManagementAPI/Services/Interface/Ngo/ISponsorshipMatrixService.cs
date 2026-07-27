using ScholarshipManagementAPI.DTOs.Ngo.SponsorshipMatrix;

namespace ScholarshipManagementAPI.Services.Interface.Ngo
{
    public interface ISponsorshipMatrixService
    {
        Task<SponsorshipMatrixDto> GetMatrixAsync();

        Task<bool> ToggleAsync(SponsorshipMatrixToggleRequestDto dto, long loginId);
    }
}
