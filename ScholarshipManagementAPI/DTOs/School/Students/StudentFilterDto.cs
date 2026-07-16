using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.School.Students
{
    public class StudentFilterDto : BaseFilterDto
    {
        public long? StudentId { get; set; }

        public long? SchoolId { get; set; }

        public long? NationalityId { get; set; }

        public long? ResidenceCountryId { get; set; }

        public long? ReligionId { get; set; }

        public long? GenderId { get; set; }

        public bool? FromDaSchool { get; set; }

        public bool? IsOrphan { get; set; }

        public bool? IsDraft { get; set; }

        public bool? IsActive { get; set; }

        public long? FinancialNeedStatusId { get; set; }

        public long? SelfRelianceLevelId { get; set; }

        public long? MotivationLevelId { get; set; }

        public long? FutureGoalsLevelId { get; set; }

        public DateTime? DobFrom { get; set; }

        public DateTime? DobTo { get; set; }


        public string? HsSpecialization { get; set; }
        public long? StudentStatusId { get; set; }



    }
}
