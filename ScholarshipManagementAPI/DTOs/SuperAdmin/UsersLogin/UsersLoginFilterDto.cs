using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLogin
{
    public class UsersLoginFilterDto :BaseFilterDto
    {
        public bool? IsActive { get; set; }


        //public int? LoginType { get; set; }
        //public long? UniversityId { get; set; }
        //public long? SchoolListId { get; set; }
    }
}
