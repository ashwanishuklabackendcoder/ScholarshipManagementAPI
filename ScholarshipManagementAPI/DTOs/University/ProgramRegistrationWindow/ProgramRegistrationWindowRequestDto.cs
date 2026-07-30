namespace ScholarshipManagementAPI.DTOs.University.ProgramRegistrationWindow
{
    public class ProgramRegistrationWindowRequestDto
    {
        public long? Id { get; set; }                    // null for update, not null for create

        public long ProgramId { get; set; }

        public int SemesterNo { get; set; }

        public DateTime RegistrationFrom { get; set; }

        public DateTime RegistrationTo { get; set; }

        public string? Notes { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }


        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }


        // response
        public string? ProgramName { get; set; }

        public string? CreatedByName { get; set; }

        public string? UpdatedByName { get; set; }

    }
}
