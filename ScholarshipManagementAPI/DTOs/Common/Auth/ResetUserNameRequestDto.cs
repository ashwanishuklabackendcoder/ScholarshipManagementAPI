namespace ScholarshipManagementAPI.DTOs.Common.Auth
{
    public class ResetUserNameRequestDto
    {
        //public long LoginId { get; set; }

        // LoginName -> UserName
        public string LoginName { get; set; } = string.Empty;
    }
}
