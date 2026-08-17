using ScholarshipManagementAPI.Helper.Enums;

namespace ScholarshipManagementAPI.DTOs.Ngo.Accreditation
{
    public class UniversityAccreditationDto
    {
        public long UniversityId { get; set; }

        public AccreditationStatusEnum AccreditationStatus { get; set; }

        public string? CommitteeComment { get; set; }

        public long UpdatedBy { get; set; }
    }
}
