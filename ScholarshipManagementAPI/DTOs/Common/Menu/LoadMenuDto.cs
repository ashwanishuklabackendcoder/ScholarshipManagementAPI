namespace ScholarshipManagementAPI.DTOs.Common.Menu
{
    public class LoadMenuDto
    {
        public long MenuLinkId { get; set; }
        public string PageHeading { get; set; } = string.Empty;
        public string ActualName { get; set; } = string.Empty;
        public string PagePath { get; set; } = string.Empty;

        public bool IsDashboard { get; set; }
        public int SequenceNo { get; set; }

        public MenuPermissionDto Permissions { get; set; } = new();
        public bool HasAccess => Permissions.CanView;       // Simple route guard helper for UI


        public long ModuleId { get; set; }                  // Helps module-wise rendering / icons
        public string? Icon { get; set; }                   // UI icon (material / fontawesome key)

        public List<LoadMenuDto> SubMenus { get; set; } = new();
    }
}
