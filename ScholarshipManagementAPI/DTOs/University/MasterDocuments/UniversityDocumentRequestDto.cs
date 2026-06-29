using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.University.MasterDocuments
{
    public class UniversityDocumentRequestDto
    {
        public long? UniversityDocsId { get; set; } // null for create, value for update 

        [Required(ErrorMessage = "University is required")]
        public long UniversityId { get; set; }

        [Required(ErrorMessage = "Downloadable is required")]
        public bool IsDownloadable { get; set; }

        [Required(ErrorMessage = "Document name is required")]
        [StringLength(200, ErrorMessage = "Document name cannot exceed 200 characters")]
        public string DocumentName { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "File name cannot exceed 200 characters")]
        public string? FileName { get; set; }

        [StringLength(200, ErrorMessage = "Document type cannot exceed 200 characters")]
        public string? DocType { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }


        // for response
        public string? UniversityName { get; set; }

    }
}
