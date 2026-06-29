namespace ScholarshipManagementAPI.DTOs.Common.Auth
{
    public class LoginRequestDto
    {
        public string LoginName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
