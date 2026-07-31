namespace ScholarshipManagementAPI.DTOs.University.AcademicRegistration
{
    public class RegisterStudentRequestDto
    {
        public long ApplicationId { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string? Remarks { get; set; }


        public int SemesterNo { get; set; }
    }
}
