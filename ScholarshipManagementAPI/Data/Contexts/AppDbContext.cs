using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.DbModels;

namespace ScholarshipManagementAPI.Data.Contexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AcCurrencyConversion> AcCurrencyConversions { get; set; }

    public virtual DbSet<AdminEmailTemplate> AdminEmailTemplates { get; set; }

    public virtual DbSet<HrStaffMaster> HrStaffMasters { get; set; }

    public virtual DbSet<KfCourse> KfCourses { get; set; }

    public virtual DbSet<KfCourseFaculty> KfCourseFaculties { get; set; }

    public virtual DbSet<KfDocumentType> KfDocumentTypes { get; set; }

    public virtual DbSet<KfFaculty> KfFaculties { get; set; }

    public virtual DbSet<KfProgram> KfPrograms { get; set; }

    public virtual DbSet<KfProgramCost> KfProgramCosts { get; set; }

    public virtual DbSet<KfProgramCourse> KfProgramCourses { get; set; }

    public virtual DbSet<KfProgramDocument> KfProgramDocuments { get; set; }

    public virtual DbSet<KfSponsorshipType> KfSponsorshipTypes { get; set; }

    public virtual DbSet<MasterDonorList> MasterDonorLists { get; set; }

    public virtual DbSet<KfSchool> KfSchools { get; set; }

    public virtual DbSet<UnUniversityRegistration> UnUniversityRegistrations { get; set; }

    public virtual DbSet<StudentDatum> StudentData { get; set; }

    public virtual DbSet<StudentDocument> StudentDocuments { get; set; }

    public virtual DbSet<StudentReqList> StudentReqLists { get; set; }

    public virtual DbSet<UnCourseReq> UnCourseReqs { get; set; }

    public virtual DbSet<UnMasterCourse> UnMasterCourses { get; set; }

    public virtual DbSet<UnMasterCourseType> UnMasterCourseTypes { get; set; }

    public virtual DbSet<UnMasterDoc> UnMasterDocs { get; set; }

    public virtual DbSet<UnUniversityList> UnUniversityLists { get; set; }

    public virtual DbSet<UsersLogin> UsersLogins { get; set; }

    public virtual DbSet<UsersLoginRole> UsersLoginRoles { get; set; }

    public virtual DbSet<UsersLoginsLog> UsersLoginsLogs { get; set; }

    public virtual DbSet<UsersMenu> UsersMenus { get; set; }

    public virtual DbSet<UsersModule> UsersModules { get; set; }

    public virtual DbSet<UsersRole> UsersRoles { get; set; }

    public virtual DbSet<UsersRolePage> UsersRolePages { get; set; }

    public virtual DbSet<ZzGeneralSetting> ZzGeneralSettings { get; set; }

    public virtual DbSet<ZzLabel> ZzLabels { get; set; }

    public virtual DbSet<ZzMasterCountry> ZzMasterCountries { get; set; }

    public virtual DbSet<ZzMasterCurrency> ZzMasterCurrencies { get; set; }

    public virtual DbSet<ZzMasterDropDown> ZzMasterDropDowns { get; set; }

    public virtual DbSet<StudentRegistration> StudentRegistrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcCurrencyConversion>(entity =>
        {
            entity.HasKey(e => e.CurrencyConversionId);

            entity.ToTable("AcCurrencyConversion");

            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Rates).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Remarks).HasMaxLength(500);

            entity.HasOne(d => d.Currency).WithMany(p => p.AcCurrencyConversions)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AcCurrencyConversion_ZzMasterCurrency");
        });

        modelBuilder.Entity<AdminEmailTemplate>(entity =>
        {
            entity.HasKey(e => e.EmailTempId)
                .HasName("PK_EmailTemplate")
                .HasFillFactor(80);

            entity.ToTable("AdminEmailTemplate");

            entity.Property(e => e.EmailTempId).HasColumnName("EmailTempID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.Subject)
                .HasMaxLength(1000)
                .HasDefaultValue("");
            entity.Property(e => e.Template).HasDefaultValue("");
            entity.Property(e => e.TemplateName)
                .HasMaxLength(200)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<HrStaffMaster>(entity =>
        {
            entity.HasKey(e => e.StaffId);

            entity.ToTable("HrStaffMaster");

            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MobileNo).HasMaxLength(100);
            entity.Property(e => e.OfficeEmail).HasMaxLength(100);
            entity.Property(e => e.PermAddress).HasMaxLength(200);
            entity.Property(e => e.PermCity).HasMaxLength(100);
            entity.Property(e => e.PermState).HasMaxLength(100);
            entity.Property(e => e.PermZipCode).HasMaxLength(50);
            entity.Property(e => e.PersonelEmail).HasMaxLength(100);
            entity.Property(e => e.Photo).HasMaxLength(200);
            entity.Property(e => e.PremCountry).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.StaffFirstName).HasMaxLength(100);
            entity.Property(e => e.StaffLastName).HasMaxLength(100);
            entity.Property(e => e.StaffSalutation).HasMaxLength(100);
            entity.Property(e => e.StaffType).HasComment("university, school, ngo");

            entity.HasOne(d => d.School).WithMany(p => p.HrStaffMasters)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_Staff_School");

            entity.HasOne(d => d.StaffTypeNavigation).WithMany(p => p.HrStaffMasters)
                .HasForeignKey(d => d.StaffType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HrStaff_StaffType");

            entity.HasOne(d => d.University).WithMany(p => p.HrStaffMasters)
                .HasForeignKey(d => d.UniversityId)
                .HasConstraintName("FK_Staff_University");
        });

        modelBuilder.Entity<KfCourse>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__kf_cours__C92D71A706135DFF");

            entity.ToTable("kf_courses");

            entity.Property(e => e.CourseCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CourseNameAr).HasMaxLength(300);
            entity.Property(e => e.CourseNameEn).HasMaxLength(300);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfCourseCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_courses_CreatedBy_UsersLogin");

            entity.HasOne(d => d.University).WithMany(p => p.KfCourses)
                .HasForeignKey(d => d.UniversityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_courses_UniversityId_UnUniversityList");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfCourseUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_courses_UpdatedBy_UsersLogin");
        });

        modelBuilder.Entity<KfCourseFaculty>(entity =>
        {
            entity.HasKey(e => e.CourseFacultyId).HasName("PK__kf_cours__651FB9999D690814");

            entity.ToTable("kf_course_faculties");

            entity.HasOne(d => d.Course).WithMany(p => p.KfCourseFaculties)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_course_faculties_Course");

            entity.HasOne(d => d.Faculty).WithMany(p => p.KfCourseFaculties)
                .HasForeignKey(d => d.FacultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_course_faculties_Faculty");
        });

        modelBuilder.Entity<KfDocumentType>(entity =>
        {
            entity.HasKey(e => e.DocumentTypeId).HasName("PK__kf_docum__DBA390E17F1AF084");

            entity.ToTable("kf_document_types");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DefaultRequired).HasDefaultValue(true);
            entity.Property(e => e.DocumentName).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfDocumentTypeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_document_types_CreatedBy_UsersLogin");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfDocumentTypeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_document_types_UpdatedBy_UsersLogin");
        });

        modelBuilder.Entity<KfFaculty>(entity =>
        {
            entity.HasKey(e => e.FacultyId).HasName("PK__kf_facul__306F630E1B2A0CAA");

            entity.ToTable("kf_faculties");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FacultyCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FacultyName).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfFacultyCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_faculties_CreatedBy_UsersLogin");

            entity.HasOne(d => d.University).WithMany(p => p.KfFaculties)
                .HasForeignKey(d => d.UniversityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_faculties_UniversityId_UnUniversityList");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfFacultyUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_faculties_UpdatedBy_UsersLogin");
        });

        modelBuilder.Entity<KfProgram>(entity =>
        {
            entity.HasKey(e => e.ProgramId).HasName("PK__kf_progr__75256058DE5E1DE3");

            entity.ToTable("kf_programs");

            entity.Property(e => e.AccreditationStatus).HasDefaultValue((byte)0);
            entity.Property(e => e.CommitteeComment).HasMaxLength(2000);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
            entity.Property(e => e.MinAcceptanceRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ProgramCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProgramName).HasMaxLength(300);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfProgramCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_programs_CreatedBy_UsersLogin");

            entity.HasOne(d => d.Faculty).WithMany(p => p.KfPrograms)
                .HasForeignKey(d => d.FacultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_programs_FacultyId_Faculties");

            entity.HasOne(d => d.University).WithMany(p => p.KfPrograms)
                .HasForeignKey(d => d.UniversityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_programs_UniversityId_UnUniversityList");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfProgramUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_programs_UpdatedBy_UsersLogin");
        });

        modelBuilder.Entity<KfProgramCost>(entity =>
        {
            entity.HasKey(e => e.ProgramCostId).HasName("PK__kf_progr__C57E046D355450BE");

            entity.ToTable("kf_program_costs");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Program).WithMany(p => p.KfProgramCosts)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_program_costs_Program");

            entity.HasOne(d => d.SponsorshipType).WithMany(p => p.KfProgramCosts)
                .HasForeignKey(d => d.SponsorshipTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_program_costs_SponsorshipType");
        });

        modelBuilder.Entity<KfProgramCourse>(entity =>
        {
            entity.HasKey(e => e.ProgramCourseId).HasName("PK__kf_progr__8BD8F31E5D79C385");

            entity.ToTable("kf_program_courses");

            entity.Property(e => e.SemesterNo).HasDefaultValue(1);

            entity.HasOne(d => d.Course).WithMany(p => p.KfProgramCourses)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_program_courses_Course");

            entity.HasOne(d => d.Program).WithMany(p => p.KfProgramCourses)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_program_courses_Program");
        });

        modelBuilder.Entity<KfProgramDocument>(entity =>
        {
            entity.HasKey(e => e.ProgramDocumentId).HasName("PK__kf_progr__6C73C4769D555FF3");

            entity.ToTable("kf_program_documents");

            entity.Property(e => e.IsRequired).HasDefaultValue(true);

            entity.HasOne(d => d.DocumentType).WithMany(p => p.KfProgramDocuments)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_program_documents_DocumentType");

            entity.HasOne(d => d.Program).WithMany(p => p.KfProgramDocuments)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_program_documents_Program");
        });

        modelBuilder.Entity<KfSponsorshipType>(entity =>
        {
            entity.HasKey(e => e.SponsorshipTypeId).HasName("PK__kf_spons__E06B5E93DE97DEE2");

            entity.ToTable("kf_sponsorship_types");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.SponsorshipName).HasMaxLength(200);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfSponsorshipTypeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_sponsorship_types_CreatedBy_UsersLogin");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfSponsorshipTypeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_sponsorship_types_UpdatedBy_UsersLogin");
        });

        modelBuilder.Entity<MasterDonorList>(entity =>
        {
            entity.HasKey(e => e.DonorId).HasName("PK__MasterDo__052E3F781454C3D8");

            entity.ToTable("MasterDonorList");

            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DonorCode).HasMaxLength(50);
            entity.Property(e => e.DonorEmail).HasMaxLength(100);
            entity.Property(e => e.DonorName).HasMaxLength(200);
            entity.Property(e => e.DonorPhone).HasMaxLength(50);
        });

        modelBuilder.Entity<KfSchool>(entity =>
        {
            entity.HasKey(e => e.SchoolId);

            entity.ToTable("kf_schools");

            entity.Property(e => e.SchoolName).HasMaxLength(300);
            entity.Property(e => e.ShortName).HasMaxLength(50);
            entity.Property(e => e.OwningInstitution).HasMaxLength(300);
            entity.Property(e => e.Area).HasMaxLength(200);
            entity.Property(e => e.CenterName).HasMaxLength(200);
            entity.Property(e => e.SchoolNumber).HasMaxLength(100);
            entity.Property(e => e.SchoolWebsite).HasMaxLength(500);
            entity.Property(e => e.SchoolPhoneNo).HasMaxLength(50);
            entity.Property(e => e.EmailId).HasMaxLength(250);
            entity.Property(e => e.PrincipalName).HasMaxLength(200);
            entity.Property(e => e.PrincipalMobile).HasMaxLength(50);
            entity.Property(e => e.PrincipalEmail).HasMaxLength(250);
            entity.Property(e => e.SchoolCoordinatorName).HasMaxLength(200);
            entity.Property(e => e.SchoolCoordinatorMobile).HasMaxLength(50);
            entity.Property(e => e.SchoolCoordinatorEmail).HasMaxLength(250);
            entity.Property(e => e.StudentCodeFormatPrefix).HasMaxLength(20);
            entity.Property(e => e.StudentCodeFormatSuffix).HasMaxLength(20);
            entity.Property(e => e.ReligionSubjectCurriculum).HasMaxLength(500);
            entity.Property(e => e.CommitteeComment).HasMaxLength(2000);

            entity.HasOne(d => d.AccreditationByNavigation).WithMany(p => p.KfSchoolAccreditationByNavigations)
                .HasForeignKey(d => d.AccreditationBy)
                .HasConstraintName("FK_kf_schools_AccreditationBy_UsersLogin");

            entity.HasOne(d => d.Country).WithMany(p => p.KfSchools)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_schools_country");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfSchoolCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_schools_CreatedBy_UsersLogin");

            entity.HasOne(d => d.DefaultCurrency).WithMany(p => p.KfSchools)
                .HasForeignKey(d => d.DefaultCurrencyId)
                .HasConstraintName("FK_kf_schools_currency");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfSchoolUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_schools_UpdatedBy_UsersLogin");
        });

        modelBuilder.Entity<UnUniversityRegistration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId);

            entity.ToTable("UnUniversityRegistration");

            entity.Property(e => e.UniversityName).HasMaxLength(300);
            entity.Property(e => e.UniversityType).HasMaxLength(100);
            entity.Property(e => e.CharterAccreditation).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(150);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Website).HasMaxLength(250);
            entity.Property(e => e.VcName).HasMaxLength(200);
            entity.Property(e => e.VcEmail).HasMaxLength(150);
            entity.Property(e => e.VcMobile).HasMaxLength(50);
            entity.Property(e => e.CoordName).HasMaxLength(200);
            entity.Property(e => e.CoordPosition).HasMaxLength(150);
            entity.Property(e => e.CoordEmail).HasMaxLength(150);
            entity.Property(e => e.CoordPhone).HasMaxLength(50);
            entity.Property(e => e.StudentsGender).HasMaxLength(50);
            entity.Property(e => e.FteRatio).HasMaxLength(50);
            entity.Property(e => e.ExternalGrants).HasMaxLength(500);

            entity.Property(e => e.IntlStudentsPct).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.OpSustainabilityPct).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.EmployabilityPct).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PhdStaffPct).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TeachingLoadHours).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Country).WithMany(p => p.UnUniversityRegistrations)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnUniversityRegistration_Country");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UnUniversityRegistrationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnUniversityRegistration_CreatedBy_UsersLogin");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.UnUniversityRegistrationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UnUniversityRegistration_UpdatedBy_UsersLogin");
        });

        modelBuilder.Entity<StudentDatum>(entity =>
        {
            entity.HasKey(e => e.StudentId);

            entity.HasIndex(e => e.SchoolId, "IX_StudentData_SchoolID");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.AddressCity).HasMaxLength(200);
            entity.Property(e => e.ClearTargetsFutureGoals).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.EmailId)
                .HasMaxLength(200)
                .HasColumnName("EmailID");
            entity.Property(e => e.EnglishPlacementTest).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.FatherName).HasMaxLength(200);
            entity.Property(e => e.Grade).HasMaxLength(50);
            entity.Property(e => e.GraduationScore).HasMaxLength(50);
            entity.Property(e => e.GuardianName).HasMaxLength(200);
            entity.Property(e => e.HighSchoolDiv).HasMaxLength(50);
            entity.Property(e => e.MasterCountry).HasMaxLength(200);
            entity.Property(e => e.MasterState).HasMaxLength(200);
            entity.Property(e => e.MaxMarks).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.MobileNo).HasMaxLength(500);
            entity.Property(e => e.MotLevelToOverComedStudying).HasMaxLength(200);
            entity.Property(e => e.MotherName).HasMaxLength(200);
            entity.Property(e => e.Nationality).HasMaxLength(200);
            entity.Property(e => e.Nin)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NIN");
            entity.Property(e => e.OrphanNumber).HasMaxLength(500);
            entity.Property(e => e.Photo)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RecommendationLetter).HasMaxLength(200);
            entity.Property(e => e.RecommendationLetterPath)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.SelfDettoSuccess).HasMaxLength(200);
            entity.Property(e => e.SocialEcoStatus).HasMaxLength(200);
            entity.Property(e => e.StudentFirstName).HasMaxLength(500);
            entity.Property(e => e.StudentLastName).HasMaxLength(500);
            entity.Property(e => e.StudentNumber).HasMaxLength(100);
            entity.Property(e => e.StudentOtherName).HasMaxLength(200);
            entity.Property(e => e.StudentSalutation).HasMaxLength(100);
            entity.Property(e => e.TanzComb).HasMaxLength(50);
            entity.Property(e => e.Tribe).HasMaxLength(200);
            entity.Property(e => e.ZipCode).HasMaxLength(50);

            entity.HasOne(d => d.School).WithMany(p => p.StudentData)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentData_MasterSchoolList");
        });

        modelBuilder.Entity<StudentDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId);

            entity.ToTable("StudentDocument");

            entity.HasIndex(e => e.StudentId, "IX_StudentDocument_StudentId");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DocName).HasMaxLength(200);
            entity.Property(e => e.DocType).HasMaxLength(100);
            entity.Property(e => e.FileUrlName).HasMaxLength(1000);

            entity.HasOne(d => d.MasterDoc).WithMany(p => p.StudentDocuments)
                .HasForeignKey(d => d.MasterDocId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentDocument_MasterDoc");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentDocuments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentDocument_StudentData");

            entity.HasOne(d => d.StudentReq).WithMany(p => p.StudentDocuments)
                .HasForeignKey(d => d.StudentReqId)
                .HasConstraintName("FK_StudentDocument_StudentReq");
        });

        modelBuilder.Entity<StudentReqList>(entity =>
        {
            entity.HasKey(e => e.StudentReqId);

            entity.ToTable("StudentReqList");

            entity.HasIndex(e => e.ReqId, "IX_StudentReqList_ReqId");

            entity.HasIndex(e => e.StudentId, "IX_StudentReqList_StudentID");

            entity.Property(e => e.StudentReqId).HasColumnName("StudentReqID");
            entity.Property(e => e.CreateEmailBy).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DaStatusDate).HasColumnType("datetime");
            entity.Property(e => e.LetterAccepCode).HasMaxLength(200);
            entity.Property(e => e.MissedDocuments).HasMaxLength(500);
            entity.Property(e => e.ReasonInProgress).HasMaxLength(500);
            entity.Property(e => e.ReasonRejection).HasMaxLength(500);
            entity.Property(e => e.SemesterStartDate).HasColumnType("datetime");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.UniStatusDate).HasColumnType("datetime");

            entity.HasOne(d => d.Donor).WithMany(p => p.StudentReqLists)
                .HasForeignKey(d => d.DonorId)
                .HasConstraintName("FK_StudentReqList_MasterDonorList");

            entity.HasOne(d => d.Req).WithMany(p => p.StudentReqLists)
                .HasForeignKey(d => d.ReqId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentReqList_UnCourseReq");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentReqLists)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentReqList_StudentData");
        });

        modelBuilder.Entity<UnCourseReq>(entity =>
        {
            entity.HasKey(e => e.ReqId);

            entity.ToTable("UnCourseReq");

            entity.Property(e => e.AcademicYear).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HsDivision).HasMaxLength(200);
            entity.Property(e => e.ReqEndDate).HasColumnType("datetime");
            entity.Property(e => e.ReqStartDate).HasColumnType("datetime");
            entity.Property(e => e.RequiredDocuments).HasMaxLength(500);
            entity.Property(e => e.TzStuCombi).HasMaxLength(50);

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.UnCourseReqs).HasForeignKey(d => d.ApprovedBy);

            entity.HasOne(d => d.Course).WithMany(p => p.UnCourseReqs)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnCourseReq_UnMasterCourse");
        });

        modelBuilder.Entity<UnMasterCourse>(entity =>
        {
            entity.HasKey(e => e.CourseId);

            entity.ToTable("UnMasterCourse");

            entity.Property(e => e.CourseCode).HasMaxLength(200);
            entity.Property(e => e.CourseName).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.DurationUnit).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Remarks).HasMaxLength(500);

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.UnMasterCourses).HasForeignKey(d => d.ApprovedBy);

            entity.HasOne(d => d.CourseType).WithMany(p => p.UnMasterCourses)
                .HasForeignKey(d => d.CourseTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnMasterCourse_AdminCourseType");

            entity.HasOne(d => d.University).WithMany(p => p.UnMasterCourses)
                .HasForeignKey(d => d.UniversityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnMasterCourse_MasterUniversityList");
        });

        modelBuilder.Entity<UnMasterCourseType>(entity =>
        {
            entity.HasKey(e => e.CourseTypeId);

            entity.ToTable("UnMasterCourseType");

            entity.Property(e => e.CourseTypeName).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.UnMasterCourseTypes).HasForeignKey(d => d.ApprovedBy);

            entity.HasOne(d => d.University).WithMany(p => p.UnMasterCourseTypes)
                .HasForeignKey(d => d.UniversityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnMasterCourseType_UnUniversityList");
        });

        modelBuilder.Entity<UnMasterDoc>(entity =>
        {
            entity.HasKey(e => e.UniversityDocsId);

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.DocType).HasMaxLength(200);
            entity.Property(e => e.DocumentName).HasMaxLength(200);
            entity.Property(e => e.FileName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");

            entity.HasOne(d => d.University).WithMany(p => p.UnMasterDocs)
                .HasForeignKey(d => d.UniversityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnMasterDocs_MasterUniversityList");
        });

        modelBuilder.Entity<UnUniversityList>(entity =>
        {
            entity.HasKey(e => e.UniversityId);

            entity.ToTable("UnUniversityList");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.UniversityName).HasMaxLength(500);

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.UnUniversityLists)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK_UnUniversityList_HrStaffMaster");

            entity.HasOne(d => d.Country).WithMany(p => p.UnUniversityLists)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnUniversityList_ZzMasterCountry");

            entity.HasOne(d => d.DefaultCurrency).WithMany(p => p.UnUniversityLists)
                .HasForeignKey(d => d.DefaultCurrencyId)
                .HasConstraintName("FK_University_Currency");
        });

        modelBuilder.Entity<UsersLogin>(entity =>
        {
            entity.HasKey(e => e.LoginId);

            entity.ToTable("UsersLogin");

            entity.HasIndex(e => e.LoginName, "UQ_UsersLogin_LoginName").IsUnique();

            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ForgotEmail).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Language)
                .HasMaxLength(50)
                .HasColumnName("language");
            entity.Property(e => e.LoginName).HasMaxLength(200);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.TempPassDateTime).HasColumnType("datetime");
            entity.Property(e => e.TempPassword).HasMaxLength(200);

            entity.HasOne(d => d.Staff).WithMany(p => p.UsersLogins)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_UsersLogin_HrStaffMaster");
        });

        modelBuilder.Entity<UsersLoginRole>(entity =>
        {
            entity.HasKey(e => e.UserLoginRoleId);

            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.Login).WithMany(p => p.UsersLoginRoles)
                .HasForeignKey(d => d.LoginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersLoginRoles_UsersLogin");

            entity.HasOne(d => d.Role).WithMany(p => p.UsersLoginRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersLoginRoles_UsersRole");
        });

        modelBuilder.Entity<UsersLoginsLog>(entity =>
        {
            entity.HasKey(e => e.LoginLogId);

            entity.ToTable("UsersLoginsLog");

            entity.Property(e => e.BrowserName).HasMaxLength(200);
            entity.Property(e => e.ComputerName).HasMaxLength(200);
            entity.Property(e => e.IpAddress).HasMaxLength(200);
            entity.Property(e => e.LoginDateTime).HasColumnType("datetime");
            entity.Property(e => e.LogoutDateTime).HasColumnType("datetime");
            entity.Property(e => e.OperatingSystem).HasMaxLength(200);
            entity.Property(e => e.UserName).HasMaxLength(200);

            entity.HasOne(d => d.Login).WithMany(p => p.UsersLoginsLogs)
                .HasForeignKey(d => d.LoginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersLoginsLog_UsersLogin");
        });

        modelBuilder.Entity<UsersMenu>(entity =>
        {
            entity.HasKey(e => e.MenuLinkId);

            entity.ToTable("UsersMenu");

            entity.HasIndex(e => e.ActualName, "UQ_UsersMenu_ActualName").IsUnique();

            entity.Property(e => e.ActualName).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Icon).HasMaxLength(500);
            entity.Property(e => e.IsView).HasDefaultValue(true);
            entity.Property(e => e.PageHeading).HasMaxLength(200);
            entity.Property(e => e.PagePath).HasMaxLength(200);
            entity.Property(e => e.ShowInMenu).HasDefaultValue(true);

            entity.HasOne(d => d.Module).WithMany(p => p.UsersMenus)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersMenu_UsersModule");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_UsersMenu_UsersMenu");
        });

        modelBuilder.Entity<UsersModule>(entity =>
        {
            entity.HasKey(e => e.ModuleId);

            entity.ToTable("UsersModule");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModuleName).HasMaxLength(200);
        });

        modelBuilder.Entity<UsersRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("UsersRole");

            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RoleName).HasMaxLength(200);

            entity.HasOne(d => d.DashboardMenuLink).WithMany(p => p.UsersRoles)
                .HasForeignKey(d => d.DashboardMenuLinkId)
                .HasConstraintName("FK_UsersRole_UsersMenu");

            entity.HasOne(d => d.Module).WithMany(p => p.UsersRoles)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersRole_UsersModule");
        });

        modelBuilder.Entity<UsersRolePage>(entity =>
        {
            entity.HasKey(e => e.RoleFormId);

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.MenuLink).WithMany(p => p.UsersRolePages)
                .HasForeignKey(d => d.MenuLinkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersRolePages_UsersMenu");

            entity.HasOne(d => d.Role).WithMany(p => p.UsersRolePages)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersRolePages_UsersRoles");
        });

        modelBuilder.Entity<ZzGeneralSetting>(entity =>
        {
            entity.HasKey(e => e.ConfigId);

            entity.Property(e => e.ConfigId).HasColumnName("ConfigID");
            entity.Property(e => e.ConfigDescription).HasMaxLength(500);
            entity.Property(e => e.ConfigKey).HasMaxLength(200);
            entity.Property(e => e.ConfigValue).HasMaxLength(200);
        });

        modelBuilder.Entity<ZzLabel>(entity =>
        {
            entity.HasKey(e => e.LabelName);

            entity.Property(e => e.LabelName).HasMaxLength(500);
            entity.Property(e => e.Arabic).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LabelId)
                .ValueGeneratedOnAdd()
                .HasColumnName("LabelID");
            entity.Property(e => e.LabelNewValue).HasMaxLength(500);
            entity.Property(e => e.LabelValue).HasMaxLength(500);
        });

        modelBuilder.Entity<ZzMasterCountry>(entity =>
        {
            entity.HasKey(e => e.CountryId);

            entity.ToTable("ZzMasterCountry");

            entity.Property(e => e.CountryId).ValueGeneratedNever();
            entity.Property(e => e.CountryAlphaCode3).HasMaxLength(5);
            entity.Property(e => e.CountryName).HasMaxLength(200);
            entity.Property(e => e.CurrencyAbb).HasMaxLength(10);
            entity.Property(e => e.CurrencyFracUnit).HasMaxLength(250);
            entity.Property(e => e.CurrencyName).HasMaxLength(50);
            entity.Property(e => e.CurrencySymbol).HasMaxLength(250);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<ZzMasterCurrency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId);

            entity.ToTable("ZzMasterCurrency");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrencyCode).HasMaxLength(10);
            entity.Property(e => e.CurrencyFracUnit).HasMaxLength(250);
            entity.Property(e => e.CurrencyName).HasMaxLength(50);
            entity.Property(e => e.CurrencySymbol).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<ZzMasterDropDown>(entity =>
        {
            entity.HasKey(e => e.UniqueId);

            entity.ToTable("ZzMasterDropDown");

            entity.Property(e => e.UniqueId).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.DisplayText).HasMaxLength(500);
            entity.Property(e => e.IsEditable).HasDefaultValue(true);
            entity.Property(e => e.IsShow).HasDefaultValue(true);
            entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);

            entity.HasOne(d => d.Module).WithMany(p => p.ZzMasterDropDowns)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("FK_ZzMasterDropDown_UsersModule");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_ZzMasterDropDown_ZzMasterDropDown");
        });

        modelBuilder.Entity<StudentRegistration>(entity =>
        {
            entity.HasKey(e => e.StudentId);
            entity.ToTable("StudentRegistration");
            entity.Property(e => e.FirstName).HasMaxLength(200);
            entity.Property(e => e.LastName).HasMaxLength(200);
            entity.Property(e => e.TotalScore).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaxScore).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RelativeGrade).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.EnglishScore).HasColumnType("decimal(5, 1)");
            entity.Property(e => e.TransferGpa).HasColumnType("decimal(4, 2)");

            entity.HasOne(d => d.ResidenceCountryNavigation)
                .WithMany()
                .HasForeignKey(d => d.ResidenceCountryId)
                .HasConstraintName("FK_StudentRegistration_ZzMasterCountry");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
