using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class StudentDatum
{
    public long StudentId { get; set; }

    public long SchoolId { get; set; }

    public string StudentNumber { get; set; } = null!;

    public string? StudentSalutation { get; set; }

    public string StudentFirstName { get; set; } = null!;

    public string? StudentLastName { get; set; }

    public string? StudentOtherName { get; set; }

    public string? Nin { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int? Gender { get; set; }

    public string? Tribe { get; set; }

    public string? Nationality { get; set; }

    public string? Address { get; set; }

    public string? AddressCity { get; set; }

    public string? MasterState { get; set; }

    public string? MasterCountry { get; set; }

    public string? ZipCode { get; set; }

    public string? MobileNo { get; set; }

    public string? EmailId { get; set; }

    public string? Photo { get; set; }

    public bool? IsOrphan { get; set; }

    public string? OrphanNumber { get; set; }

    public long? Religion { get; set; }

    public string? GraduationScore { get; set; }

    public string? Grade { get; set; }

    public string? HighSchoolDiv { get; set; }

    public string? TanzComb { get; set; }

    public string? FatherName { get; set; }

    public string? MotherName { get; set; }

    public string? GuardianName { get; set; }

    public string? SocialEcoStatus { get; set; }

    public string? RecommendationLetter { get; set; }

    public string? SelfDettoSuccess { get; set; }

    public string? MotLevelToOverComedStudying { get; set; }

    public string? ClearTargetsFutureGoals { get; set; }

    public decimal? MaxMarks { get; set; }

    public decimal? EnglishPlacementTest { get; set; }

    public string? RecommendationLetterPath { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual KfSchool School { get; set; } = null!;

    public virtual ICollection<StudentDocument> StudentDocuments { get; set; } = new List<StudentDocument>();

    public virtual ICollection<StudentReqList> StudentReqLists { get; set; } = new List<StudentReqList>();
}
