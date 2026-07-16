using ScholarshipManagementAPI.DTOs.Ngo.Accreditation;
using ScholarshipManagementAPI.Helper.Enums;

namespace ScholarshipManagementAPI.Services.Interface.Ngo
{
    public interface IAccreditationService
    {

        Task<bool> ApproveSchoolAsync(long schoolId, int approvalStatus, long approvedBy);

        Task<bool> ApproveUniversityAsync(long id, int approvalStatus, long approvedBy);

        Task<bool> AccreditateProgram(ProgramAccreditationDto dto);


    }
}
