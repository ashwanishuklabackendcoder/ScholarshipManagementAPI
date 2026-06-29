namespace ScholarshipManagementAPI.DTOs.Common.Filter
{
    public class BaseFilterDto
    {
        public int PageNumber { get; set; } = 1;   // default first page
        public int PageSize { get; set; } = 25;    // default page size // 0 = fetch all

        public string? SearchText { get; set; }     // global search text
    }
}
