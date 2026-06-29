namespace ScholarshipManagementAPI.DTOs.SuperAdmin.Label
{
    public class LabelRequestDto
    {
        public long? LableId { get; set; }   // 0 = Create, >0 = Update

        public string LabelName { get; set; } = string.Empty;

        public string LabelValue { get; set; } = string.Empty;

        public string? LabelNewValue { get; set; }

        public string? Arabic { get; set; }

        public DateTime CreatedDate { get; set; } // dd-mm-yyyy hh:mm:ss

        public string CreatedBy { get; set; } = string.Empty;
    }
}
