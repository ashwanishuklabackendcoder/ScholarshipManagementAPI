namespace ScholarshipManagementAPI.DTOs.Common.Settings
{
    public class AwsS3OptionsDto
    {
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }
}
