namespace ScholarshipManagementAPI.DTOs.University.CourseRequirement
{
    public class CourseRequirementEnrollmentDto
    {
        public long ReqId { get; set; }

        public string? CourseName { get; set; }
        public string? UniversityName { get; set; }

        public string? AcademicYear { get; set; }
        public string? RequiredDocuments { get; set; }


        public int Seats { get; set; }
        public int StudentCount { get; set; }
        public int RemainingSeats { get; set; }


        public DateTime? ReqStartDate { get; set; }
        public DateTime? ReqEndDate { get; set; }


        public double? TotalCost { get; set; }


    }
}
