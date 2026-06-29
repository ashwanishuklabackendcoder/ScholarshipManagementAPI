namespace ScholarshipManagementAPI.DTOs.Common.Auth
{
    public class UpdateMyProfileDto
    {
        // 🔹 Basic Info
        public string Saluatation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Mobile { get; set; }
        public string? PersonalEmail { get; set; }

        // 🔹 Address
        public string? Address { get; set; }
        public string? City { get; set; }
        //public string? State { get; set; }
        public string? Country { get; set; }
        public string? Zip { get; set; }
    }
}
