using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.Faculties;
using ScholarshipManagementAPI.DTOs.University.MasterUniversity;

namespace ScholarshipManagementAPI.Services.Interface.University
{
    public interface IFacultiesService
    {
        Task<long> CreateAsync(FacultyRequestDto dto);
        Task<bool> UpdateAsync(FacultyRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<FacultyRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<FacultyRequestDto>> GetByFilterAsync(FacultyFilterDto filter);


        // ---------------- GET FACULTY PROGRAMS DASHBOARD ----------------
        Task<FacultyProgramsDashboardDto> GetFacultyProgramsDashboardAsync(long universityId);

    }
}
