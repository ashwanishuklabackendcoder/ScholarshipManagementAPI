using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.Common.Auth
{
    public class UserIdentifierDto
    {
        // public string EmailOrMobile { get; set; } = string.Empty;


        public string EmailOrUsername { get; set; } = string.Empty;
    }
}
