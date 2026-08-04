using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRole
{
    public class UsersRoleByModulesRequestDto
    {
        [Required]
        public List<long> ModuleIds { get; set; } = new();
    }
}
