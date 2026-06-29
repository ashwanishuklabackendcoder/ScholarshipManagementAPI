namespace ScholarshipManagementAPI.DTOs.Common.Menu
{
    public class MenuPermissionDto
    {
        public bool CanView { get; set; }
        public bool CanInsert { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}
