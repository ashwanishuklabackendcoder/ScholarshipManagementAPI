namespace ScholarshipManagementAPI.DTOs.University.Faculties
{
    public class FacultyProgramsDashboardDto
    {
        public int TotalFaculties { get; set; }

        public int AccreditedPrograms { get; set; }

        public int UnderReviewPrograms { get; set; }

        public List<FacultyProgramsSummaryDto> Faculties { get; set; } = [];
    }
}
