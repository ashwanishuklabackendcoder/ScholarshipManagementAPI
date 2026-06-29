namespace ScholarshipManagementAPI.DTOs.University.Faculties
{
    public class FacultyProgramItemDto
    {
        public long ProgramId { get; set; }

        public string ProgramName { get; set; } = string.Empty;

        public string ProgramCode { get; set; } = string.Empty;

        public bool IsDraft { get; set; }

        public byte? AccreditationStatus { get; set; }

        public string StatusName { get; set; } = string.Empty;

    }
}
