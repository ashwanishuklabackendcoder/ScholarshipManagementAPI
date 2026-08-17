using ScholarshipManagementAPI.Helper.Enums;

namespace ScholarshipManagementAPI.DTOs.Ngo.Accreditation
{
    public class SchoolAccreditationDto
    {
        public long SchoolId { get; set; }

        public AccreditationStatusEnum AccreditationStatus { get; set; }

        public string? CommitteeComment { get; set; }

        public long UpdatedBy { get; set; }
    }
}
