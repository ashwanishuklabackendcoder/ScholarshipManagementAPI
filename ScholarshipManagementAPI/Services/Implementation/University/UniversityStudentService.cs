using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.Students;
using ScholarshipManagementAPI.DTOs.University.Students;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Services.Interface.University;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class UniversityStudentService : IUniversityStudentService
    {
        private readonly AppDbContext _context;

        public UniversityStudentService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<UniversityStudentRequestDto?> GetByIdAsync(long studentId, long loginId, LoggedInUserDto currentUser)
        {
            var universityId = currentUser.UniversityId
                ?? throw new UnauthorizedAccessException("User is not associated with a university.");

            var item = await _context.StudentProgramApplications
                .AsNoTracking()
                .Where(x =>
                    x.StudentId == studentId &&
                    x.Program.UniversityId == universityId &&
                    x.ApplicationStatus != (int)StudentApplicationStatus.Draft)
                .OrderByDescending(x => x.SubmittedDate ?? x.AppliedDate)
                .Select(x => new UniversityStudentRequestDto
                {
                    // Student
                    StudentId = x.StudentId,
                    StudentCode = x.Student.StudentCode,
                    PhotoPath = x.Student.PhotoPath,

                    FirstName = x.Student.FirstName,
                    SecondName = x.Student.SecondName,
                    ThirdName = x.Student.ThirdName,
                    LastName = x.Student.LastName,

                    // Personal Information
                    MotherName = x.Student.MotherName,
                    DateOfBirth = x.Student.Dob.HasValue
    ? x.Student.Dob.Value.ToDateTime(TimeOnly.MinValue)
    : null,

                    GenderId = x.Student.GenderId,
                    GenderName = x.Student.Gender != null
    ? x.Student.Gender.DisplayText
    : null,

                    ReligionId = x.Student.ReligionId,
                    ReligionName = x.Student.Religion != null
    ? x.Student.Religion.DisplayText
    : null,

                    Nationality = x.Student.Nationality != null
    ? x.Student.Nationality.CountryName
    : null,

                    CountryOfResidence = x.Student.ResidenceCountry != null
    ? x.Student.ResidenceCountry.CountryName
    : null,

                    IsDirectAidOrphan = x.Student.IsOrphan,
                    OrphanNumber = x.Student.OrphanNumber,

                    // Contact Information
                    PhoneNumber = x.Student.Phone,
                    EmailAddress = x.Student.Email,

                    City = x.Student.City,
                    Village = x.Student.Village,
                    Block = x.Student.Block,
                    Street = x.Student.Street,

                    // School / Academic Information
                    SchoolName = x.Student.School != null
    ? x.Student.School.SchoolName
    : null,

                    TotalScore = x.Student.TotalScore,
                    EnglishScore = x.Student.EnglishScore,

                    HsSpecialization = x.Student.HsSpecialization,
                    TanzanianStudentCombination = x.Student.TanzanianStudentCombination,


                    // Application
                    ApplicationId = x.ApplicationId,

                    ApplicationStatusId = x.ApplicationStatus,

                    ApplicationStatusName =
    ((StudentApplicationStatus)x.ApplicationStatus).ToString(),

                    ActionDate =
    x.UpdatedDate ??
    x.SubmittedDate ??
    x.AppliedDate,

                    // Program
                    ProgramId = x.ProgramId,
                    ProgramName = x.Program.ProgramName,
                    ProgramCode = x.Program.ProgramCode,

                    // Faculty
                    FacultyId = x.Program.FacultyId,
                    FacultyName = x.Program.Faculty.FacultyName,

                    // University
                    UniversityId = x.Program.UniversityId,
                    UniversityName = x.Program.University.UniversityName,


                    // Permissions
                    //CanView = true,
                    //CanApprove = x.ApplicationStatus == (int)StudentApplicationStatus.AcceptanceInProcess,
                    //CanReject = x.ApplicationStatus == (int)StudentApplicationStatus.AcceptanceInProcess,
                    //CanRegister = x.ApplicationStatus == (int)StudentApplicationStatus.Awarded,
                    //CanGraduate = x.ApplicationStatus == (int)StudentApplicationStatus.Registered,
                    //CanEdit = false
                })
                .FirstOrDefaultAsync();

            if (item == null)
                return null;

            item.FullName = string.Join(" ",
                new[]
                {
            item.FirstName,
            item.SecondName,
            item.ThirdName,
            item.LastName
                }
                .Where(s => !string.IsNullOrWhiteSpace(s)));

            return item;
        }


        public async Task<PagedResultDto<UniversityStudentRequestDto>> GetByFilterAsync(UniversityStudentFilterDto filter , LoggedInUserDto currentUser)
        {
            var query = _context.StudentProgramApplications
                .AsNoTracking()
                .AsQueryable();


            // Resolve logged in user's university
            var universityId = currentUser.UniversityId
               ?? throw new UnauthorizedAccessException("User is not associated with a university.");

            query = query.Where(x => x.Program.UniversityId == universityId);


            query = query.Where(x => x.ApplicationStatus != (long)StudentApplicationStatus.Draft);


            if (filter.FacultyId.HasValue)
            {
                query = query.Where(x =>  x.Program.FacultyId == filter.FacultyId);
            }

            if (filter.ProgramId.HasValue)
            {
                query = query.Where(x =>  x.ProgramId == filter.ProgramId);
            }

            if (filter.StudentStatusId.HasValue)
            {
                query = query.Where(x => x.ApplicationStatus == filter.StudentStatusId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim();

                query = query.Where(x =>
                    x.Student.FirstName.Contains(search) ||
                    (x.Student.SecondName != null && x.Student.SecondName.Contains(search)) ||
                    (x.Student.ThirdName != null && x.Student.ThirdName.Contains(search)) ||
                    x.Student.LastName.Contains(search) ||
                    (x.Student.Email != null && x.Student.Email.Contains(search)) ||
                    (x.Student.Phone != null && x.Student.Phone.Contains(search)) ||
                    (x.Student.DaStudentCode != null && x.Student.DaStudentCode.Contains(search)) ||
                    x.Student.StudentCode.Contains(search) ||

                    x.Program.ProgramName.Contains(search) ||
                    x.Program.ProgramCode.Contains(search) ||
                    x.Program.Faculty.FacultyName.Contains(search)
                );
            }

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(x => x.SubmittedDate ?? x.AppliedDate);

            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new UniversityStudentRequestDto
                {
                    // Student
                    StudentId = x.StudentId,
                    StudentCode = x.Student.StudentCode,
                    PhotoPath = x.Student.PhotoPath,

                    FirstName = x.Student.FirstName,
                    SecondName = x.Student.SecondName,
                    ThirdName = x.Student.ThirdName,
                    LastName = x.Student.LastName,

                    SchoolName = x.Student.School != null
                        ? x.Student.School.SchoolName
                        : null,

                    TotalScore = x.Student.TotalScore,
                    EnglishScore = x.Student.EnglishScore,
                    HsSpecialization = x.Student.HsSpecialization,
                    TanzanianStudentCombination = x.Student.TanzanianStudentCombination,

                    // Application
                    ApplicationId = x.ApplicationId,

                    ApplicationStatusId = x.ApplicationStatus,

                    ApplicationStatusName =
                        ((StudentApplicationStatus)x.ApplicationStatus).ToString(),

                    ActionDate =
                        x.UpdatedDate ??
                        x.SubmittedDate ??
                        x.AppliedDate,

                    // Program
                    ProgramId = x.ProgramId,
                    ProgramName = x.Program.ProgramName,
                    ProgramCode = x.Program.ProgramCode,

                    // Faculty
                    FacultyId = x.Program.FacultyId,
                    FacultyName = x.Program.Faculty.FacultyName,

                    // University
                    UniversityId = x.Program.UniversityId,
                    UniversityName = x.Program.University.UniversityName,

                })
                .ToListAsync();



            foreach (var item in items)
            {
                item.FullName = string.Join(" ",
                    new[]
                    {
                item.FirstName,
                item.SecondName,
                item.ThirdName,
                item.LastName
                    }
                    .Where(s => !string.IsNullOrWhiteSpace(s)));
            }

            return new PagedResultDto<UniversityStudentRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }





    }




}
