namespace ScholarshipManagementAPI.DTOs.Common.Response
{
    public class ApiResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Result { get; set; }
    }
}
