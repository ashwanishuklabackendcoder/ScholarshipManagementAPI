using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentRequirements;
using ScholarshipManagementAPI.DTOs.School.Students;
using ScholarshipManagementAPI.DTOs.University.CourseRequirement;
using ScholarshipManagementAPI.DTOs.University.MasterCourse;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Implementation.SuperAdmin;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;
using ScholarshipManagementAPI.Services.Interface.University;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class CourseRequirementService : ICourseRequirementService
    {
        private readonly AppDbContext _context;
        private readonly ICurrencyConversionService _currencyConversionService;
        private readonly IGeneralSettingsService _generalSettingsService;

        public CourseRequirementService(AppDbContext context, ICurrencyConversionService currencyConversionService, IGeneralSettingsService generalSettingsService)
        {
            _context = context;
            _currencyConversionService = currencyConversionService;
            _generalSettingsService = generalSettingsService;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(CourseRequirementRequestDto dto, LoggedInUserDto currentUser)
        {
            var (baseCurrency, rate) = await GetCurrencyContextAsync(currentUser.DefaultCurrencyCode);

            var entity = new UnCourseReq
            {
                CourseId = dto.CourseId,
                AcademicYear = dto.AcademicYear,

                RequiredDocuments = dto.RequiredDocuments,
                HsDivision = dto.HsDivision,

                NoSeats = dto.NoSeats,
                CollegeScore = dto.CollegeScore,
                TzStuCombi = dto.TzStuCombi,

                RegistrationCost = dto.RegistrationCost,
                TutionCost = dto.TutionCost,
                TextBookCost = dto.TextBookCost,
                AccomoCost = dto.AccomoCost,
                TravellingCost = dto.TravellingCost,
                TransportCost = dto.TransportCost,
                DocuAttestCost = dto.DocuAttestCost,
                VisaResiCost = dto.VisaResiCost,

                ReqStartDate = dto.ReqStartDate,
                ReqEndDate = dto.ReqEndDate,

                ApprovalStatus = (int)ApprovalStatus.Pending,  // pending by default
                ApprovedBy = null,                             // no approver at creation
                IsActive = true,                               // default to active
                CreatedDate = dto.CreatedDate,                 // always server-side
                CreatedBy = dto.CreatedBy                      // always server-side
            };



            // Apply cost conversion
            ApplyCostConversion(dto, entity, currentUser.DefaultCurrencyCode, baseCurrency, rate);

            _context.UnCourseReqs.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ReqId;
        }



        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(CourseRequirementRequestDto dto, LoggedInUserDto currentUser)
        {
            var entity = await _context.UnCourseReqs
                .FirstOrDefaultAsync(x => x.ReqId == dto.ReqId);

            if (entity == null)
            {
                throw new CustomException("Course Requirement not found");
            }

            // ⭐ BUSINESS RULE CHECK (place here)
            if (entity.ApprovalStatus == (int)ApprovalStatus.Approved)
                throw new CustomException("Approved course requirement cannot be edited");

            // ⭐ BUSINESS RULE CHECK (place here)
            if (entity.ApprovalStatus == (int)ApprovalStatus.Rejected)
                throw new CustomException("Rejected course requirement cannot be edited");


            var (baseCurrency, rate) = await GetCurrencyContextAsync(currentUser.DefaultCurrencyCode);

            // CourseId usually should NOT be changed
            // entity.CourseId = dto.CourseId;

            entity.AcademicYear = dto.AcademicYear;

            entity.RequiredDocuments = dto.RequiredDocuments;
            entity.HsDivision = dto.HsDivision;

            entity.NoSeats = dto.NoSeats;
            entity.CollegeScore = dto.CollegeScore;
            entity.TzStuCombi = dto.TzStuCombi;

            entity.RegistrationCost = dto.RegistrationCost;
            entity.TutionCost = dto.TutionCost;
            entity.TextBookCost = dto.TextBookCost;
            entity.AccomoCost = dto.AccomoCost;
            entity.TravellingCost = dto.TravellingCost;
            entity.TransportCost = dto.TransportCost;
            entity.DocuAttestCost = dto.DocuAttestCost;
            entity.VisaResiCost = dto.VisaResiCost;

            entity.ReqStartDate = dto.ReqStartDate;
            entity.ReqEndDate = dto.ReqEndDate;

            // entity.IsActive = dto.IsActive;

            // entity.ApprovalStatus = dto.ApprovalStatus;   // usually updated via ngo
            // entity.ApprovedBy = dto.ApprovedBy;           // usually updated via ngo
            // entity.CreatedDate = dto.CreatedDate;         // usually not updated
            // entity.CreatedBy = dto.CreatedBy;             // usually not updated


            // Apply cost conversion
            ApplyCostConversion(dto, entity, currentUser.DefaultCurrencyCode, baseCurrency, rate);

            await _context.SaveChangesAsync();

            return true;
        }



        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.UnCourseReqs
                .FirstOrDefaultAsync(x => x.ReqId == id && x.IsActive == true);

            if (entity == null)
                return false;


            if (entity.ApprovalStatus == (int)ApprovalStatus.Approved)
                throw new CustomException("Approved course requirement cannot be deleted");

            // ⭐ BUSINESS RULE CHECK (place here)
            if (entity.ApprovalStatus == (int)ApprovalStatus.Rejected)
                throw new CustomException("Rejected course requirement cannot be deleted");


            // Soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }



        // ---------------- GET BY ID ----------------
        public async Task<CourseRequirementRequestDto?> GetByIdAsync(long id, LoggedInUserDto currentUser)
        {
            var dto = await _context.UnCourseReqs
                .AsNoTracking()
                .Where(x => x.ReqId == id)
                .Include(x => x.Course)
                .Select(x => new CourseRequirementRequestDto
                {
                    ReqId = x.ReqId,
                    CourseId = x.CourseId,
                    AcademicYear = x.AcademicYear,

                    RequiredDocuments = x.RequiredDocuments,
                    HsDivision = x.HsDivision,

                    NoSeats = x.NoSeats,
                    CollegeScore = x.CollegeScore,
                    TzStuCombi = x.TzStuCombi,

                    RegistrationCost = x.RegistrationCost,
                    TutionCost = x.TutionCost,
                    TextBookCost = x.TextBookCost,
                    AccomoCost = x.AccomoCost,
                    TravellingCost = x.TravellingCost,
                    TransportCost = x.TransportCost,
                    DocuAttestCost = x.DocuAttestCost,
                    VisaResiCost = x.VisaResiCost,

                    ReqStartDate = x.ReqStartDate,
                    ReqEndDate = x.ReqEndDate,

                    IsActive = x.IsActive ?? false,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,

                    CourseName = x.Course.CourseName,
                    UniversityName = x.Course.University.UniversityName,

                    ApprovalStatus = x.ApprovalStatus,
                    ApprovedBy = x.ApprovedBy,
                    ApprovedByName = x.ApprovedByNavigation != null ? x.ApprovedByNavigation.LoginName : null
                })
                .FirstOrDefaultAsync();


            if (dto == null)
                return null;

            var (baseCurrency, rate) = await GetCurrencyContextAsync(currentUser.DefaultCurrencyCode);

            return ConvertCostsFromBase(dto, currentUser.DefaultCurrencyCode, baseCurrency, rate);
        }



        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<CourseRequirementRequestDto>> GetByFilterAsync(CourseRequirementFilterDto filter, LoggedInUserDto currentUser)
        {
            var query = _context.UnCourseReqs
                .AsNoTracking()
                .Include(x => x.Course) 
                .AsQueryable();

            // ---------- DATA SCOPE FILTER ----------
            if (currentUser.StaffType != StaffType.SuperAdmin &&
                currentUser.StaffType != StaffType.Ngo)
            {
                if (currentUser.StaffType == StaffType.University)
                {
                    query = query.Where(x => x.Course.UniversityId == currentUser.UniversityId);
                }
                else if (currentUser.StaffType == StaffType.School)
                {
                    // School should only view course requirements
                    query = query.Where(x => true);
                }
            }

            // UniversityId filter
            if (filter.UniversityId.HasValue)
            {
                query = query.Where(x => x.Course.UniversityId == filter.UniversityId.Value);
            }

            // CourseId filter
            if (filter.CourseId.HasValue)
            {
                query = query.Where(x => x.CourseId == filter.CourseId.Value);
            }

            // Approved filter
            if (filter.ApprovalStatus.HasValue)
            {
                query = query.Where(x => x.ApprovalStatus == filter.ApprovalStatus.Value);
            }

            // Active status filter
            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
            }


            // Date range filter (OVERLAP LOGIC)
            if (filter.ReqStartDate.HasValue)
            {
                var startDate = filter.ReqStartDate.Value.Date;

                query = query.Where(x =>
                    x.ReqEndDate == null || x.ReqEndDate.Value.Date >= startDate
                );
            }

            if (filter.ReqEndDate.HasValue)
            {
                var endDate = filter.ReqEndDate.Value.Date;

                query = query.Where(x =>
                    x.ReqStartDate == null || x.ReqStartDate.Value.Date <= endDate
                );
            }


            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.Course.CourseName.ToLower().Contains(search) 
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.ReqId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var data = await query
                .Select(x => new CourseRequirementRequestDto
                {
                    ReqId = x.ReqId,
                    CourseId = x.CourseId,
                    AcademicYear = x.AcademicYear,

                    RequiredDocuments = x.RequiredDocuments,
                    HsDivision = x.HsDivision,

                    NoSeats = x.NoSeats,
                    CollegeScore = x.CollegeScore,
                    TzStuCombi = x.TzStuCombi,

                    RegistrationCost = x.RegistrationCost,
                    TutionCost = x.TutionCost,
                    TextBookCost = x.TextBookCost,
                    AccomoCost = x.AccomoCost,
                    TravellingCost = x.TravellingCost,
                    TransportCost = x.TransportCost,
                    DocuAttestCost = x.DocuAttestCost,
                    VisaResiCost = x.VisaResiCost,

                    ReqStartDate = x.ReqStartDate,
                    ReqEndDate = x.ReqEndDate,

                    IsActive = x.IsActive ?? false,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,

                    CourseName = x.Course.CourseName,
                    UniversityName = x.Course.University.UniversityName,

                    ApprovalStatus = x.ApprovalStatus,
                    ApprovedBy = x.ApprovedBy,
                    ApprovedByName = x.ApprovedByNavigation != null ? x.ApprovedByNavigation.LoginName : null
                })
                .ToListAsync();

            var (baseCurrency, rate) = await GetCurrencyContextAsync(currentUser.DefaultCurrencyCode);

            var items = data
                .Select(dto => ConvertCostsFromBase(dto, currentUser.DefaultCurrencyCode, baseCurrency, rate))
                .ToList();

            return new PagedResultDto<CourseRequirementRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }



        // ---------------- GET ALL Enrollments ----------------
        public async Task<PagedResultDto<CourseRequirementEnrollmentDto>> GetEnrollmentsAsync(CourseRequirementFilterDto filter, LoggedInUserDto currentUser)
        {
            var query = _context.UnCourseReqs
                .AsNoTracking()
                .Include(x => x.Course)
                .ThenInclude(c => c.University)
                .AsQueryable();

            // ---------- DATA SCOPE FILTER ----------
            if (currentUser.StaffType != StaffType.SuperAdmin &&
                currentUser.StaffType != StaffType.Ngo)
            {
                if (currentUser.StaffType == StaffType.University)
                {
                    query = query.Where(x => x.Course.UniversityId == currentUser.UniversityId);
                }
            }

            // ---------- FILTERS ----------
            if (filter.UniversityId.HasValue)
                query = query.Where(x => x.Course.UniversityId == filter.UniversityId.Value);

            if (filter.CourseId.HasValue)
                query = query.Where(x => x.CourseId == filter.CourseId.Value);

            if (filter.ApprovalStatus.HasValue)
                query = query.Where(x => x.ApprovalStatus == filter.ApprovalStatus.Value);

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive.Value);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.Course.CourseName.ToLower().Contains(search));
            }

            // ---------- TOTAL COUNT ----------
            var totalCount = await query.CountAsync();

            // ---------- PAGINATION ----------
            query = query.OrderByDescending(x => x.ReqId);

            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            // ---------- SELECT (ENROLLMENT DATA) ----------
            var data = await query
                .Select(x => new CourseRequirementEnrollmentDto
                {
                    ReqId = x.ReqId,

                    CourseName = x.Course.CourseName,
                    UniversityName = x.Course.University.UniversityName,

                    AcademicYear = x.AcademicYear,
                    Seats = x.NoSeats,

                    ReqStartDate = x.ReqStartDate,
                    ReqEndDate = x.ReqEndDate,

                    // Student Count
                    StudentCount = _context.StudentReqLists
                        .Count(s => s.ReqId == x.ReqId),

                    // Remaining Seats
                    RemainingSeats = (x.NoSeats) -
                        _context.StudentReqLists.Count(s => s.ReqId == x.ReqId),

                    RequiredDocuments = x.RequiredDocuments,

                    TotalCost =
                        (x.RegistrationCost ?? 0) +
                        (x.TutionCost ?? 0) +
                        (x.TextBookCost ?? 0) +
                        (x.AccomoCost ?? 0) +
                        (x.TravellingCost ?? 0) +
                        (x.TransportCost ?? 0) +
                        (x.DocuAttestCost ?? 0) +
                        (x.VisaResiCost ?? 0)
                })
                .ToListAsync();

            var (baseCurrency, rate) = await GetCurrencyContextAsync(currentUser.DefaultCurrencyCode);

            var items = data
                .Select(dto => ConvertEnrollmentCost(dto, currentUser.DefaultCurrencyCode, baseCurrency, rate))
                .ToList();

            return new PagedResultDto<CourseRequirementEnrollmentDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }




        // ---------------- GET ALL Enrolled Students ----------------
        public async Task<PagedResultDto<EnrolledStudentDto>> GetEnrolledStudentsAsync(long? reqId, StudentRequirementFilterDto filter, LoggedInUserDto currentUser)
        {
            var query = _context.StudentReqLists
                .AsNoTracking()
                .Include(x => x.Donor)
                .Include(x => x.Student)
                .ThenInclude(s => s.School)
                .AsQueryable();


            // Apply reqId filter ONLY if provided
            if (reqId.HasValue)
            {
                query = query.Where(x => x.ReqId == reqId.Value);
            }

            // ---------- DATA SCOPE FILTER ----------
            //if (currentUser.StaffType == StaffType.Ngo)
            //{ 
            //    query = query.Where(x => x.UniAwardingstatus == (int)AwardingStatus.Awarded);
            //}


            // ---------- Filters ----------
            if (filter.SchoolId.HasValue)
            {
                query = query.Where(x => x.Student.SchoolId == filter.SchoolId.Value);
            }

            if (filter.UniAwardingStatus.HasValue)
            {
                query = query.Where(x => x.UniAwardingstatus == (int)filter.UniAwardingStatus.Value);
            }

            if (filter.DaAdmissionStatus.HasValue)
            {
                query = query.Where(x => x.DaAdmissionStatus == (int)filter.DaAdmissionStatus.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();

                query = query.Where(x =>
                    x.Student.StudentNumber.ToLower().Contains(search) ||
                    x.Student.StudentFirstName.ToLower().Contains(search) ||
                    (x.Student.StudentLastName != null && x.Student.StudentLastName.ToLower().Contains(search)) ||
                    (x.Student.EmailId != null && x.Student.EmailId.ToLower().Contains(search))
                );
            }

            // ---------- Count ----------
            var totalCount = await query.CountAsync();

            // ---------- Paging ----------
            query = query.OrderByDescending(x => x.CreatedDate);

            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            // ---------- Projection ----------
            var items = await query
                .Select(x => new EnrolledStudentDto
                {
                    StudentId = x.StudentId,
                    ReqId = x.ReqId,
                    StudentFullName = x.Student.StudentFirstName + " " + (x.Student.StudentLastName ?? ""),
                    StudentNumber = x.Student.StudentNumber,
                    StudentPhoto = x.Student.Photo,

                    Gender = x.Student.Gender,
                    DateOfBirth = x.Student.DateOfBirth,

                    StudentEmail = x.Student.EmailId,
                    StudentMobileNo = x.Student.MobileNo,

                    Address = x.Student.Address,
                    AddressCity = x.Student.AddressCity,
                    MasterState = x.Student.MasterState,
                    MasterCountry = x.Student.MasterCountry,
                    ZipCode = x.Student.ZipCode,


                    SchoolName = x.Student.School != null
                        ? x.Student.School.SchoolName
                        : null,

                    ShortName = x.Student.School != null
                        ? x.Student.School.ShortName
                        : null,

                    SchoolEmail = x.Student.School != null ? x.Student.School.EmailId : null,
                    SchoolMobileNo = x.Student.School != null ? x.Student.School.SchoolPhoneNo : null,
                    SchoolWebsite = x.Student.School != null ? x.Student.School.SchoolWebsite : null,

                    // Status mapping
                    DocStatus = x.DocumentStatus == (int)DocumentStatus.Accepted ? "Accepted" :
                    x.DocumentStatus == (int)DocumentStatus.Rejected ? "Rejected" :
                    x.DocumentStatus == (int)DocumentStatus.InProcess ? "In Process" : "",

                    AwardingStatus = x.UniAwardingstatus == (int)AwardingStatus.Awarded ? "Awarded" :
                    x.UniAwardingstatus == (int)AwardingStatus.Rejected ? "Rejected" :
                    x.UniAwardingstatus == (int)AwardingStatus.InProcess ? "In Process" : "",

                    SponsoredStatus = x.DaAdmissionStatus == (int)SponsoredStatus.Sponsored ? "Sponsored" :
                    x.DaAdmissionStatus == (int)SponsoredStatus.Rejected ? "Rejected" :
                    x.DaAdmissionStatus == (int)SponsoredStatus.InProcess ? "In Process" : "",

                    DonorName = x.Donor != null ? x.Donor.DonorName : null,
                    DonorId = x.DonorId,
                    TotalCost = x.TotalCost,

                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            return new PagedResultDto<EnrolledStudentDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }







        private async Task<(string baseCurrency, decimal rate)> GetCurrencyContextAsync(string userCurrency)
        {
            var config = await _generalSettingsService.GetGeneralConfigAsync();
            var baseCurrency = config.BaseCurrencyCode;

            decimal rate = 1;

            if (userCurrency != baseCurrency)
            {
                rate = await _currencyConversionService.GetRateAsync(userCurrency);
            }

            return (baseCurrency, rate);
        }


        private double? ConvertToBase(double? value, string userCurrency, string baseCurrency, decimal rate)
        {
            return value.HasValue
                ? (double?)_currencyConversionService.ConvertToBase(
                    Convert.ToDecimal(value.Value),
                    userCurrency,
                    baseCurrency,
                    rate)
                : null;
        }



        private void ApplyCostConversion( 
            CourseRequirementRequestDto dto,
            UnCourseReq entity,
            string userCurrency,
            string baseCurrency,
            decimal rate)
        {
            entity.RegistrationCost = ConvertToBase(dto.RegistrationCost, userCurrency, baseCurrency, rate);
            entity.TutionCost = ConvertToBase(dto.TutionCost, userCurrency, baseCurrency, rate);
            entity.TextBookCost = ConvertToBase(dto.TextBookCost, userCurrency, baseCurrency, rate);
            entity.AccomoCost = ConvertToBase(dto.AccomoCost, userCurrency, baseCurrency, rate);
            entity.TravellingCost = ConvertToBase(dto.TravellingCost, userCurrency, baseCurrency, rate);
            entity.TransportCost = ConvertToBase(dto.TransportCost, userCurrency, baseCurrency, rate);
            entity.DocuAttestCost = ConvertToBase(dto.DocuAttestCost, userCurrency, baseCurrency, rate);
            entity.VisaResiCost = ConvertToBase(dto.VisaResiCost, userCurrency, baseCurrency, rate);
        }





        private double? ConvertFromBase(double? value, string userCurrency, string baseCurrency, decimal rate)
        {
            return value.HasValue
                ? (double?)_currencyConversionService.ConvertFromBase(
                    Convert.ToDecimal(value.Value),
                    userCurrency,
                    baseCurrency,
                    rate)
                : null;
        }


        private CourseRequirementRequestDto ConvertCostsFromBase(
            CourseRequirementRequestDto dto,
            string userCurrency,
            string baseCurrency,
            decimal rate)
        {
            dto.RegistrationCost = ConvertFromBase(dto.RegistrationCost, userCurrency, baseCurrency, rate);
            dto.TutionCost = ConvertFromBase(dto.TutionCost, userCurrency, baseCurrency, rate);
            dto.TextBookCost = ConvertFromBase(dto.TextBookCost, userCurrency, baseCurrency, rate);
            dto.AccomoCost = ConvertFromBase(dto.AccomoCost, userCurrency, baseCurrency, rate);
            dto.TravellingCost = ConvertFromBase(dto.TravellingCost, userCurrency, baseCurrency, rate);
            dto.TransportCost = ConvertFromBase(dto.TransportCost, userCurrency, baseCurrency, rate);
            dto.DocuAttestCost = ConvertFromBase(dto.DocuAttestCost, userCurrency, baseCurrency, rate);
            dto.VisaResiCost = ConvertFromBase(dto.VisaResiCost, userCurrency, baseCurrency, rate);

            return dto;
        }





        private CourseRequirementEnrollmentDto ConvertEnrollmentCost(
            CourseRequirementEnrollmentDto dto,
            string userCurrency,
            string baseCurrency,
            decimal rate)
        {
            dto.TotalCost = dto.TotalCost.HasValue
                ? (double?)_currencyConversionService.ConvertFromBase(
                    Convert.ToDecimal(dto.TotalCost.Value),
                    userCurrency,
                    baseCurrency,
                    rate)
                : null;

            return dto;
        }



    }
}
