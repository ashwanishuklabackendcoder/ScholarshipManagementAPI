using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.University.ProgramRegistrationWindow
{
    public class ProgramRegistrationWindowFilterDto : BaseFilterDto
    {
        public long? Id { get; set; }

        public long? ProgramId { get; set; }

        public long? UniversityId { get; set; }

    }

}
