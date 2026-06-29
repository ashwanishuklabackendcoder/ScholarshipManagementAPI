namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRolePage
{
    public class RolePagePermissionDto
    {
        public long MenuLinkId { get; set; }
        public long RoleId { get; set; }

        public long? RoleFormId { get; set; }   // null if not mapped

        public bool ViewPer { get; set; }
        public bool InsertPer { get; set; }
        public bool UpdatePer { get; set; }
        public bool DeletePer { get; set; }

        public string? RoleName { get; set; }
        public string? Module { get; set; }
        public string? PageHeading { get; set; }
        public string? PagePath { get; set; }


        // any permission is true => mapped, otherwise not mapped
        public bool IsMapped =>
            ViewPer || InsertPer || UpdatePer || DeletePer;



        public long? ParentId { get; set; }
        public int? LevelNo { get; set; }
        public int? SequenceNo { get; set; }

    }
}
