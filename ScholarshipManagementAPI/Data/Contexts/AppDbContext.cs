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

    public virtual DbSet<KfCourse> KfCourses { get; set; }

    public virtual DbSet<KfCourseFaculty> KfCourseFaculties { get; set; }

    public virtual DbSet<KfDocumentType> KfDocumentTypes { get; set; }

    public virtual DbSet<KfFaculty> KfFaculties { get; set; }

    public virtual DbSet<KfMarketingAdministrativeFee> KfMarketingAdministrativeFees { get; set; }

    public virtual DbSet<KfProgram> KfPrograms { get; set; }

    public virtual DbSet<KfProgramCost> KfProgramCosts { get; set; }

    public virtual DbSet<KfProgramCourse> KfProgramCourses { get; set; }

    public virtual DbSet<KfProgramDocument> KfProgramDocuments { get; set; }

    public virtual DbSet<KfProgramRegistrationWindow> KfProgramRegistrationWindows { get; set; }

    public virtual DbSet<KfSchool> KfSchools { get; set; }

    public virtual DbSet<KfSponsorshipCategoryMapping> KfSponsorshipCategoryMappings { get; set; }

    public virtual DbSet<KfSponsorshipStudentCategory> KfSponsorshipStudentCategories { get; set; }

    public virtual DbSet<KfSponsorshipType> KfSponsorshipTypes { get; set; }

    public virtual DbSet<KfStaff> KfStaffs { get; set; }

    public virtual DbSet<KfStaffSchoolCoordinatorMapping> KfStaffSchoolCoordinatorMappings { get; set; }

    public virtual DbSet<KfStaffUniversityCoordinatorMapping> KfStaffUniversityCoordinatorMappings { get; set; }

    public virtual DbSet<KfStudentAcademicRegistration> KfStudentAcademicRegistrations { get; set; }

    public virtual DbSet<KfStudentHistory> KfStudentHistories { get; set; }

    public virtual DbSet<KfStudentProgramApplication> KfStudentProgramApplications { get; set; }

    public virtual DbSet<KfStudentProgramDocument> KfStudentProgramDocuments { get; set; }

    public virtual DbSet<KfStudentRegistration> KfStudentRegistrations { get; set; }

    public virtual DbSet<MasterDonorList> MasterDonorLists { get; set; }

    public virtual DbSet<UnUniversityRegistration> UnUniversityRegistrations { get; set; }

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
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
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
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.Subject)
                .HasMaxLength(1000)
                .HasDefaultValue("");
            entity.Property(e => e.Template).HasDefaultValue("");
            entity.Property(e => e.TemplateName)
                .HasMaxLength(200)
                .HasDefaultValue("");
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
            entity.Property(e => e.IsDraft).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfCourseCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_courses_CreatedBy_UsersLogin");

            entity.HasOne(d => d.University).WithMany(p => p.KfCourses)
                .HasForeignKey(d => d.UniversityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_courses_UniversityId_UnUniversityRegistration");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfCourseUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_courses_UpdatedBy_UsersLogin");
        });

        modelBuilder.Entity<KfCourseFaculty>(entity =>
        {
            entity.HasKey(e => e.CourseFacultyId).HasName("PK__kf_cours__651FB9999D690814");

            entity.ToTable("kf_course_faculties");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);

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
            entity.Property(e => e.IsDraft).HasDefaultValue(true);

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
            entity.Property(e => e.IsDraft).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfFacultyCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_faculties_CreatedBy_UsersLogin");

            entity.HasOne(d => d.University).WithMany(p => p.KfFaculties)
                .HasForeignKey(d => d.UniversityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_faculties_UniversityId_UnUniversityRegistration");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfFacultyUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_faculties_UpdatedBy_UsersLogin");
        });

        modelBuilder.Entity<KfMarketingAdministrativeFee>(entity =>
        {
            entity.HasKey(e => e.MarketingAdministrativeFeeId).HasName("PK__kf_marke__100B757136F1207E");

            entity.ToTable("kf_marketing_administrative_fees");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.FeePercentage).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IsCurrent).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfMarketingAdministrativeFeeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfMarketingAdministrativeFeeUpdatedByNavigations).HasForeignKey(d => d.UpdatedBy);
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
                .HasConstraintName("FK_kf_programs_UniversityId_UnUniversityRegistration");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfProgramUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_programs_UpdatedBy_UsersLogin");
        });

        modelBuilder.Entity<KfProgramCost>(entity =>
        {
            entity.HasKey(e => e.ProgramCostId).HasName("PK__kf_progr__C57E046D355450BE");

            entity.ToTable("kf_program_costs");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);

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

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
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

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
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

        modelBuilder.Entity<KfProgramRegistrationWindow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__kf_progr__3214EC07B6B61B15");

            entity.ToTable("kf_program_registration_windows");

            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.RegistrationFrom).HasColumnType("datetime");
            entity.Property(e => e.RegistrationTo).HasColumnType("datetime");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfProgramRegistrationWindowCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_program_registration_windows_createdby");

            entity.HasOne(d => d.Program).WithMany(p => p.KfProgramRegistrationWindows)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_program_registration_windows_program");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfProgramRegistrationWindowUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_program_registration_windows_updatedby");
        });

        modelBuilder.Entity<KfSchool>(entity =>
        {
            entity.HasKey(e => e.SchoolId).HasName("PK__kf_schoo__3DA4675B97BF5ED5");

            entity.ToTable("kf_schools");

            entity.Property(e => e.AccreditationStatus).HasDefaultValue((byte)1);
            entity.Property(e => e.Area).HasMaxLength(200);
            entity.Property(e => e.CenterName).HasMaxLength(200);
            entity.Property(e => e.CommitteeComment).HasMaxLength(2000);
            entity.Property(e => e.EmailId).HasMaxLength(250);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
            entity.Property(e => e.OwningInstitution).HasMaxLength(300);
            entity.Property(e => e.PrincipalEmail).HasMaxLength(250);
            entity.Property(e => e.PrincipalMobile).HasMaxLength(50);
            entity.Property(e => e.PrincipalName).HasMaxLength(200);
            entity.Property(e => e.ReligionSubjectCurriculum).HasMaxLength(500);
            entity.Property(e => e.SchoolCoordinatorEmail).HasMaxLength(250);
            entity.Property(e => e.SchoolCoordinatorMobile).HasMaxLength(50);
            entity.Property(e => e.SchoolCoordinatorName).HasMaxLength(200);
            entity.Property(e => e.SchoolName).HasMaxLength(300);
            entity.Property(e => e.SchoolNumber).HasMaxLength(100);
            entity.Property(e => e.SchoolPhoneNo).HasMaxLength(50);
            entity.Property(e => e.SchoolWebsite).HasMaxLength(500);
            entity.Property(e => e.ShortName).HasMaxLength(50);
            entity.Property(e => e.StudentCodeFormatPrefix).HasMaxLength(20);
            entity.Property(e => e.StudentCodeFormatSuffix).HasMaxLength(20);
            entity.Property(e => e.StudentSequenceNumber).HasDefaultValue(1);

            entity.HasOne(d => d.AccreditationByNavigation).WithMany(p => p.KfSchoolAccreditationByNavigations)
                .HasForeignKey(d => d.AccreditationBy)
                .HasConstraintName("FK_kf_schools_AccreditationBy");

            entity.HasOne(d => d.Country).WithMany(p => p.KfSchools)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_schools_Country");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfSchoolCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_schools_CreatedBy");

            entity.HasOne(d => d.DefaultCurrency).WithMany(p => p.KfSchools)
                .HasForeignKey(d => d.DefaultCurrencyId)
                .HasConstraintName("FK_kf_schools_DefaultCurrency");

            entity.HasOne(d => d.SchoolStatusNavigation).WithMany(p => p.KfSchoolSchoolStatusNavigations)
                .HasForeignKey(d => d.SchoolStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_schools_SchoolStatus");

            entity.HasOne(d => d.SchoolTypeNavigation).WithMany(p => p.KfSchoolSchoolTypeNavigations)
                .HasForeignKey(d => d.SchoolType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_schools_SchoolType");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfSchoolUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_schools_UpdatedBy");
        });

        modelBuilder.Entity<KfSponsorshipCategoryMapping>(entity =>
        {
            entity.HasKey(e => e.MappingId);

            entity.ToTable("kf_sponsorship_category_mapping");

            entity.HasIndex(e => new { e.SponsorshipTypeId, e.StudentCategoryId }, "UQ_kf_sponsorship_category_mapping").IsUnique();

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfSponsorshipCategoryMappingCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_sponsorship_category_mapping_CreatedBy");

            entity.HasOne(d => d.SponsorshipType).WithMany(p => p.KfSponsorshipCategoryMappings)
                .HasForeignKey(d => d.SponsorshipTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_sponsorship_category_mapping_SponsorshipType");

            entity.HasOne(d => d.StudentCategory).WithMany(p => p.KfSponsorshipCategoryMappings)
                .HasForeignKey(d => d.StudentCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_sponsorship_category_mapping_StudentCategory");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfSponsorshipCategoryMappingUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_sponsorship_category_mapping_UpdatedBy");
        });

        modelBuilder.Entity<KfSponsorshipStudentCategory>(entity =>
        {
            entity.HasKey(e => e.StudentCategoryId);

            entity.ToTable("kf_sponsorship_student_categories");

            entity.Property(e => e.CategoryName).HasMaxLength(200);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfSponsorshipStudentCategoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_sponsorship_student_categories_CreatedBy_UsersLogin");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfSponsorshipStudentCategoryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_sponsorship_student_categories_UpdatedBy_UsersLogin");
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

        modelBuilder.Entity<KfStaff>(entity =>
        {
            entity.HasKey(e => e.StaffId);

            entity.ToTable("kf_staffs");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MobileNumber).HasMaxLength(100);
            entity.Property(e => e.OfficialEmail).HasMaxLength(100);
            entity.Property(e => e.PermAddress).HasMaxLength(200);
            entity.Property(e => e.PermCity).HasMaxLength(100);
            entity.Property(e => e.PermState).HasMaxLength(100);
            entity.Property(e => e.PermZipCode).HasMaxLength(50);
            entity.Property(e => e.PersonalEmail).HasMaxLength(100);
            entity.Property(e => e.Photo).HasMaxLength(200);
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.StaffFirstName).HasMaxLength(100);
            entity.Property(e => e.StaffLastName).HasMaxLength(100);
            entity.Property(e => e.StaffSalutation).HasMaxLength(100);
            entity.Property(e => e.StaffType).HasComment("university, school, ngo");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfStaffCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_staffs_CreatedBy");

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.KfStaffs)
                .HasForeignKey(d => d.Gender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_staffs_Gender_MasterDropDown");

            entity.HasOne(d => d.PermCountry).WithMany(p => p.KfStaffs)
                .HasForeignKey(d => d.PermCountryId)
                .HasConstraintName("FK_kf_staffs_Country");

            entity.HasOne(d => d.School).WithMany(p => p.KfStaffs)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_kf_staffs_School");

            entity.HasOne(d => d.StaffTypeNavigation).WithMany(p => p.KfStaffs)
                .HasForeignKey(d => d.StaffType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_staffs_StaffType");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfStaffUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_staffs_UpdatedBy");
        });

        modelBuilder.Entity<KfStaffSchoolCoordinatorMapping>(entity =>
        {
            entity.HasKey(e => e.StaffSchoolCoordinatorMappingId);

            entity.ToTable("kf_staff_school_coordinator_mapping");

            entity.HasIndex(e => e.SchoolId, "IX_HrStaffSchoolCoordinatorMapping_SchoolId");

            entity.HasIndex(e => e.StaffId, "IX_HrStaffSchoolCoordinatorMapping_StaffId");

            entity.HasIndex(e => new { e.StaffId, e.SchoolId }, "UQ_kf_staff_school_coordinator_mapping").IsUnique();

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfStaffSchoolCoordinatorMappingCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_staff_school_coordinator_mapping_CreatedBy");

            entity.HasOne(d => d.School).WithMany(p => p.KfStaffSchoolCoordinatorMappings)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_staff_school_coordinator_mapping_School");

            entity.HasOne(d => d.Staff).WithMany(p => p.KfStaffSchoolCoordinatorMappings)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_staff_school_coordinator_mapping_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfStaffSchoolCoordinatorMappingUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_staff_school_coordinator_mapping_UpdatedBy");
        });

        modelBuilder.Entity<KfStaffUniversityCoordinatorMapping>(entity =>
        {
            entity.HasKey(e => e.StaffUniversityCoordinatorMappingId);

            entity.ToTable("kf_staff_university_coordinator_mapping");

            entity.HasIndex(e => e.StaffId, "IX_HrStaffUniversityCoordinatorMapping_StaffId");

            entity.HasIndex(e => e.UniversityId, "IX_HrStaffUniversityCoordinatorMapping_UniversityId");

            entity.HasIndex(e => new { e.StaffId, e.UniversityId }, "UQ_kf_staff_university_coordinator_mapping").IsUnique();

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfStaffUniversityCoordinatorMappingCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_staff_university_coordinator_mapping_CreatedBy");

            entity.HasOne(d => d.Staff).WithMany(p => p.KfStaffUniversityCoordinatorMappings)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_staff_university_coordinator_mapping_Staff");

            entity.HasOne(d => d.University).WithMany(p => p.KfStaffUniversityCoordinatorMappings)
                .HasForeignKey(d => d.UniversityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_staff_university_coordinator_mapping_University");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfStaffUniversityCoordinatorMappingUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_staff_university_coordinator_mapping_UpdatedBy");
        });

        modelBuilder.Entity<KfStudentAcademicRegistration>(entity =>
        {
            entity.ToTable("kf_student_academic_registrations");

            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Application).WithMany(p => p.KfStudentAcademicRegistrations)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_academic_registrations_Application");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfStudentAcademicRegistrationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_academic_registrations_CreatedBy");

            entity.HasOne(d => d.Program).WithMany(p => p.KfStudentAcademicRegistrations)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_academic_registrations_Program");

            entity.HasOne(d => d.Student).WithMany(p => p.KfStudentAcademicRegistrations)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_academic_registrations_Student");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfStudentAcademicRegistrationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_student_academic_registrations_UpdatedBy");
        });

        modelBuilder.Entity<KfStudentHistory>(entity =>
        {
            entity.HasKey(e => e.StudentHistoryId);

            entity.ToTable("kf_student_history");

            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Application).WithMany(p => p.KfStudentHistories)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_kf_student_history_student_program_application");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfStudentHistories)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_history_CreatedBy");

            entity.HasOne(d => d.Student).WithMany(p => p.KfStudentHistories)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_history_kf_student_registration");
        });

        modelBuilder.Entity<KfStudentProgramApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationId);

            entity.ToTable("kf_student_program_applications");

            entity.Property(e => e.Remarks).HasMaxLength(1000);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfStudentProgramApplicationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_program_applications_CreatedBy");

            entity.HasOne(d => d.Program).WithMany(p => p.KfStudentProgramApplications)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_program_applications_kf_programs");

            entity.HasOne(d => d.Student).WithMany(p => p.KfStudentProgramApplications)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_program_applications_StudentRegistration");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfStudentProgramApplicationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_student_program_applications_UpdatedBy");
        });

        modelBuilder.Entity<KfStudentProgramDocument>(entity =>
        {
            entity.HasKey(e => e.StudentProgramDocumentId);

            entity.ToTable("kf_student_program_documents");

            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.OriginalFileName).HasMaxLength(255);
            entity.Property(e => e.ReviewerRemark).HasMaxLength(1000);
            entity.Property(e => e.StoragePath).HasMaxLength(500);
            entity.Property(e => e.StoredFileName).HasMaxLength(255);

            entity.HasOne(d => d.Application).WithMany(p => p.KfStudentProgramDocuments)
                .HasForeignKey(d => d.ApplicationId)
                .HasConstraintName("FK_kf_student_program_documents_kf_student_program_applications");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.KfStudentProgramDocuments)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_program_documents_kf_document_types");

            entity.HasOne(d => d.ProgramDocument).WithMany(p => p.KfStudentProgramDocuments)
                .HasForeignKey(d => d.ProgramDocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_program_documents_kf_program_documents");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfStudentProgramDocumentUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_student_program_documents_UpdatedBy");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.KfStudentProgramDocumentUploadedByNavigations)
                .HasForeignKey(d => d.UploadedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_program_documents_UploadedBy");
        });

        modelBuilder.Entity<KfStudentRegistration>(entity =>
        {
            entity.HasKey(e => e.StudentId);

            entity.ToTable("kf_student_registrations");

            entity.Property(e => e.Block).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasDefaultValue(2L);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DaStudentCode).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.EnglishScore).HasColumnType("decimal(5, 1)");
            entity.Property(e => e.FirstName).HasMaxLength(200);
            entity.Property(e => e.House).HasMaxLength(200);
            entity.Property(e => e.HsSpecialization).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(200);
            entity.Property(e => e.MaxScore).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MotherName).HasMaxLength(200);
            entity.Property(e => e.OrphanNumber).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(100);
            entity.Property(e => e.PhotoPath).HasMaxLength(1000);
            entity.Property(e => e.RecommendationLetterNotes).HasMaxLength(2000);
            entity.Property(e => e.RecommendationLetterPath).HasMaxLength(1000);
            entity.Property(e => e.RelativeGrade).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.SecondName).HasMaxLength(200);
            entity.Property(e => e.Street).HasMaxLength(200);
            entity.Property(e => e.StudentCode).HasMaxLength(100);
            entity.Property(e => e.TanzanianStudentCombination).HasMaxLength(300);
            entity.Property(e => e.ThirdName).HasMaxLength(200);
            entity.Property(e => e.TotalScore).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransferGpa).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.TransferInstitution).HasMaxLength(300);
            entity.Property(e => e.TransferInstitutionType).HasMaxLength(100);
            entity.Property(e => e.TransferProgram).HasMaxLength(300);
            entity.Property(e => e.Tribe).HasMaxLength(200);
            entity.Property(e => e.Village).HasMaxLength(200);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KfStudentRegistrationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_registrations_CreatedBy");

            entity.HasOne(d => d.FinancialNeedStatus).WithMany(p => p.KfStudentRegistrationFinancialNeedStatuses)
                .HasForeignKey(d => d.FinancialNeedStatusId)
                .HasConstraintName("FK_kf_student_registrations_FinancialNeedStatus");

            entity.HasOne(d => d.FutureGoalsLevel).WithMany(p => p.KfStudentRegistrationFutureGoalsLevels)
                .HasForeignKey(d => d.FutureGoalsLevelId)
                .HasConstraintName("FK_kf_student_registrations_FutureGoals");

            entity.HasOne(d => d.Gender).WithMany(p => p.KfStudentRegistrationGenders)
                .HasForeignKey(d => d.GenderId)
                .HasConstraintName("FK_kf_student_registrations_Gender");

            entity.HasOne(d => d.MotivationLevel).WithMany(p => p.KfStudentRegistrationMotivationLevels)
                .HasForeignKey(d => d.MotivationLevelId)
                .HasConstraintName("FK_kf_student_registrations_Motivation");

            entity.HasOne(d => d.Nationality).WithMany(p => p.KfStudentRegistrationNationalities)
                .HasForeignKey(d => d.NationalityId)
                .HasConstraintName("FK_kf_student_registrations_Nationality");

            entity.HasOne(d => d.Religion).WithMany(p => p.KfStudentRegistrationReligions)
                .HasForeignKey(d => d.ReligionId)
                .HasConstraintName("FK_kf_student_registrations_Religion");

            entity.HasOne(d => d.ResidenceCountry).WithMany(p => p.KfStudentRegistrationResidenceCountries)
                .HasForeignKey(d => d.ResidenceCountryId)
                .HasConstraintName("FK_kf_student_registrations_ResidenceCountry");

            entity.HasOne(d => d.School).WithMany(p => p.KfStudentRegistrations)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_kf_student_registrations_School");

            entity.HasOne(d => d.SelfRelianceLevel).WithMany(p => p.KfStudentRegistrationSelfRelianceLevels)
                .HasForeignKey(d => d.SelfRelianceLevelId)
                .HasConstraintName("FK_kf_student_registrations_SelfReliance");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.KfStudentRegistrationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_kf_student_registrations_UpdatedBy");
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
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
        });

        modelBuilder.Entity<UnUniversityRegistration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__UnUniver__6EF588100A965F01");

            entity.ToTable("UnUniversityRegistration");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CharterAccreditation).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(150);
            entity.Property(e => e.CommitteeComment).HasMaxLength(2000);
            entity.Property(e => e.CoordEmail)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CoordName).HasMaxLength(200);
            entity.Property(e => e.CoordPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoordPosition).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EmployabilityPct).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ExternalGrants).HasMaxLength(500);
            entity.Property(e => e.FteRatio).HasMaxLength(50);
            entity.Property(e => e.IntlStudentsPct).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
            entity.Property(e => e.OpSustainabilityPct).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PhdStaffPct).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TeachingLoadHours).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UniversityName).HasMaxLength(300);
            entity.Property(e => e.VcEmail)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.VcMobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VcName).HasMaxLength(200);
            entity.Property(e => e.Website)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.AccreditationByNavigation).WithMany(p => p.UnUniversityRegistrationAccreditationByNavigations)
                .HasForeignKey(d => d.AccreditationBy)
                .HasConstraintName("FK_UnUniversityRegistration_AccreditationBy_UsersLogin");

            entity.HasOne(d => d.Country).WithMany(p => p.UnUniversityRegistrations)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnUniversityRegistration_Country");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UnUniversityRegistrationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnUniversityRegistration_CreatedBy_UsersLogin");

            entity.HasOne(d => d.StudentsGenderType).WithMany(p => p.UnUniversityRegistrationStudentsGenderTypes)
                .HasForeignKey(d => d.StudentsGenderTypeId)
                .HasConstraintName("FK_UnUniversityRegistration_StudentsGenderType_ZzMasterDropDown");

            entity.HasOne(d => d.UniversityTypeNavigation).WithMany(p => p.UnUniversityRegistrationUniversityTypeNavigations)
                .HasForeignKey(d => d.UniversityType)
                .HasConstraintName("FK_UnUniversityRegistration_UniversityType_ZzMasterDropDown");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.UnUniversityRegistrationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UnUniversityRegistration_UpdatedBy_UsersLogin");
        });

        modelBuilder.Entity<UsersLogin>(entity =>
        {
            entity.HasKey(e => e.LoginId);

            entity.ToTable("UsersLogin");

            entity.HasIndex(e => e.LoginName, "UQ_UsersLogin_LoginName").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LoginName).HasMaxLength(200);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.RecoveryEmail).HasMaxLength(200);
            entity.Property(e => e.TempPassDateTime).HasColumnType("datetime");
            entity.Property(e => e.TempPassword).HasMaxLength(200);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersLogin_CreatedBy");

            entity.HasOne(d => d.Staff).WithMany(p => p.UsersLogins)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersLogin_kf_staffs");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UsersLogin_UpdatedBy");
        });

        modelBuilder.Entity<UsersLoginRole>(entity =>
        {
            entity.HasKey(e => e.UserLoginRoleId);

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UsersLoginRoleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersLoginRoles_CreatedBy");

            entity.HasOne(d => d.Login).WithMany(p => p.UsersLoginRoleLogins)
                .HasForeignKey(d => d.LoginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersLoginRoles_UsersLogin");

            entity.HasOne(d => d.Role).WithMany(p => p.UsersLoginRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersLoginRoles_UsersRole");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.UsersLoginRoleUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UsersLoginRoles_UpdatedBy");
        });

        modelBuilder.Entity<UsersLoginsLog>(entity =>
        {
            entity.HasKey(e => e.LoginLogId);

            entity.ToTable("UsersLoginsLog");

            entity.Property(e => e.BrowserName).HasMaxLength(200);
            entity.Property(e => e.ComputerName).HasMaxLength(200);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IpAddress).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
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
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
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

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModuleName).HasMaxLength(200);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UsersModuleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_UsersModule_CreatedBy");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.UsersModuleUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UsersModule_UpdatedBy");
        });

        modelBuilder.Entity<UsersRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("UsersRole");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RoleName).HasMaxLength(200);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UsersRoleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersRole_CreatedBy");

            entity.HasOne(d => d.Module).WithMany(p => p.UsersRoles)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersRole_UsersModule");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.UsersRoleUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UsersRole_UpdatedBy");
        });

        modelBuilder.Entity<UsersRolePage>(entity =>
        {
            entity.HasKey(e => e.RoleFormId);

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);

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
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
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
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
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
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CurrencyAbb).HasMaxLength(10);
            entity.Property(e => e.CurrencyFracUnit).HasMaxLength(250);
            entity.Property(e => e.CurrencyName).HasMaxLength(50);
            entity.Property(e => e.CurrencySymbol).HasMaxLength(250);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
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
            entity.Property(e => e.IsDraft).HasDefaultValue(true);
        });

        modelBuilder.Entity<ZzMasterDropDown>(entity =>
        {
            entity.HasKey(e => e.UniqueId);

            entity.ToTable("ZzMasterDropDown");

            entity.Property(e => e.UniqueId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.DisplayText).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDraft).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ZzMasterDropDownCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ZzMasterDropDown_CreatedBy_UsersLogin");

            entity.HasOne(d => d.Module).WithMany(p => p.ZzMasterDropDowns)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("FK_ZzMasterDropDown_UsersModule");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_ZzMasterDropDown_ZzMasterDropDown");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ZzMasterDropDownUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_ZzMasterDropDown_UpdatedBy_UsersLogin");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
