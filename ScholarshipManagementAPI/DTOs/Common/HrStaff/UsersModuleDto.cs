namespace ScholarshipManagementAPI.DTOs.Common.HrStaff
{
    public class UsersModuleDto
    {
        public long ModuleId { get; set; }

        public string ModuleName { get; set; } = null!;

        public bool? IsActive { get; set; }
    }
}
