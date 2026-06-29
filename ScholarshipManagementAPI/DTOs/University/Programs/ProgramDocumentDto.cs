namespace ScholarshipManagementAPI.DTOs.University.Programs
{
    public class ProgramDocumentDto
    {
        public long? ProgramDocumentId { get; set; }

        public long DocumentTypeId { get; set; }

        public string? DocumentTypeName { get; set; }

        public bool IsRequired { get; set; }

        public int? DisplayOrder { get; set; }
    }
}
