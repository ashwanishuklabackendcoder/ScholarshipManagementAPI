using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.University.MasterUniversity
{
    public class UniversityRequestDto
    {
        public long? UniversityId { get; set; }   // null for create, value for update

        [StringLength(500)]
        public string UniversityName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public long CountryId { get; set; }

        public bool IsApproved { get; set; }

        public long? ApprovedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Remarks { get; set; }

        public long? DefaultCurrencyId { get; set; }


        // Response-only helpers (optional but useful)
        public string? CountryName { get; set; }
        public string? ApprovedByName { get; set; }

    }
}
