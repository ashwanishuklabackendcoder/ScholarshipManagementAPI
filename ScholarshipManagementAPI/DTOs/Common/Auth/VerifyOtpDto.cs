namespace ScholarshipManagementAPI.DTOs.Common.Auth
{
    public class VerifyOtpDto
    {
        public string EmailOrUsername { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
