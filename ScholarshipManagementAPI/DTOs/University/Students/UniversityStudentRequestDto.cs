namespace ScholarshipManagementAPI.DTOs.University.Students
{
    public class UniversityStudentRequestDto
    {
        // Student
        public long StudentId { get; set; }
        public string? StudentCode { get; set; }
        public string? PhotoPath { get; set; }

        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }

        // Personal Information
        public string? MotherName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public long? GenderId { get; set; }
        public string? GenderName { get; set; }
        public long? ReligionId { get; set; }
        public string? ReligionName { get; set; }
        public string? Nationality { get; set; }
        public string? CountryOfResidence { get; set; }
        public bool? IsDirectAidOrphan { get; set; }
        public string? OrphanNumber { get; set; }

        // Contact Information
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? City { get; set; }
        public string? Village { get; set; }
        public string? Block { get; set; }
        public string? Street { get; set; }

        // Academic Information
        public decimal? TotalScore { get; set; }
        public decimal? EnglishScore { get; set; }
        public string? HsSpecialization { get; set; }
        public string? TanzanianStudentCombination { get; set; }

        // School Information
        public string? SchoolName { get; set; }

        // Application
        public long ApplicationId { get; set; }
        public long ApplicationStatusId { get; set; }
        public string? ApplicationStatusName { get; set; }
        public DateTime? ActionDate { get; set; }

        // Program
        public long ProgramId { get; set; }
        public string? ProgramName { get; set; }
        public string? ProgramCode { get; set; }

        // Faculty
        public long FacultyId { get; set; }
        public string? FacultyName { get; set; }

        // University
        public long UniversityId { get; set; }
        public string? UniversityName { get; set; }

        // Permissions
        public bool CanApprove { get; set; }
        public bool CanReject { get; set; }
        public bool CanRegister { get; set; }
        public bool CanGraduate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
    }
}
