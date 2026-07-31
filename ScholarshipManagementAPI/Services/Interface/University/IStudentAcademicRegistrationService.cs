using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.AcademicRegistration;

namespace ScholarshipManagementAPI.Services.Interface.University
{
    public interface IStudentAcademicRegistrationService
    {
        Task<PagedResultDto<AcademicRegistrationDto>> SearchAsync(AcademicRegistrationFilterDto filter, LoggedInUserDto currentUser);

        Task<bool> RegisterStudentAsync(RegisterStudentRequestDto dto, LoggedInUserDto currentUser);

    }
}
