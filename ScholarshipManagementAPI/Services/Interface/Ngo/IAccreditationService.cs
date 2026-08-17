using ScholarshipManagementAPI.DTOs.Ngo.Accreditation;
using ScholarshipManagementAPI.Helper.Enums;

namespace ScholarshipManagementAPI.Services.Interface.Ngo
{
    public interface IAccreditationService
    {

        Task<bool> AccreditSchoolAsync(SchoolAccreditationDto dto);

        Task<bool> AccreditUniversityAsync(UniversityAccreditationDto dto);

        Task<bool> AccreditProgramAsync(ProgramAccreditationDto dto);


    }
}
