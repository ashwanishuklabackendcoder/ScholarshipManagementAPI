namespace ScholarshipManagementAPI.DTOs.University.Faculties
{
    public class FacultyProgramsSummaryDto
    {
        public long FacultyId { get; set; }

        public string FacultyName { get; set; } = string.Empty;

        public string? FacultyCode { get; set; }

        public int TotalPrograms { get; set; }

        public decimal AverageSemesters { get; set; }

        public List<FacultyProgramItemDto> Programs { get; set; } = [];
    }
}
