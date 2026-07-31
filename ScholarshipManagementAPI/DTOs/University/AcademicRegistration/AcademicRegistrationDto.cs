namespace ScholarshipManagementAPI.DTOs.University.AcademicRegistration
{
    public class AcademicRegistrationDto
    {
        public long Id { get; set; }

        public long StudentId { get; set; }
        public string? StudentCode { get; set; }
        public string? StudentName { get; set; }
        public string? PhotoPath { get; set; }

        public long ApplicationId { get; set; }

        public long ProgramId { get; set; }
        public string? ProgramName { get; set; }

        public long FacultyId { get; set; }
        public string? FacultyName { get; set; }

        public long UniversityId { get; set; }
        public string? UniversityName { get; set; }

        public int SemesterNo { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string? Remarks { get; set; }

        public int ApplicationStatusId { get; set; }
        public string? ApplicationStatusName { get; set; }
    }
}
