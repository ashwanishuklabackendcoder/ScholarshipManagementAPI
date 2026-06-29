using ScholarshipManagementAPI.Helper.Enums;

namespace ScholarshipManagementAPI.DTOs.Common.Auth
{
    public class LoggedInUserDto
    {
        // Auth / identity
        public long LoginId { get; set; }
        public long RoleId { get; set; }
        public long ModuleId { get; set; }

        public string RoleName { get; set; } = null!;

        // RoleName
        public string UserName { get; set; } = null!;


        // Derived from module
        public StaffType StaffType { get; set; }
        public bool IsSuperAdmin => StaffType == StaffType.SuperAdmin;


        // Tenant info (from DB) - HrStaffMaster 
        public long? StaffId { get; set; }
        public long? UniversityId { get; set; }
        public long? SchoolId { get; set; }
        public long? NgoId { get; set; }



        // default currency info (for school & university) and for rest send base currency info
        public string DefaultCurrencyCode { get; set; } = "";
        public string DefaultCurrencyName { get; set; } = "";
        public string DefaultCurrencySymbol { get; set; } = "";


    }
}
