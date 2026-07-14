using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.HrStaff;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersMenu;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Common;
using static ScholarshipManagementAPI.Helper.Utilities.Constant;

namespace ScholarshipManagementAPI.Services.Implementation.Common
{
    public class StaffService : IStaffService
    {

        private readonly AppDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly ICommonService _commonService;

        public StaffService(AppDbContext context,
            INotificationService notificationService,
            ICommonService commonService)
        {
            _context = context;
            _notificationService = notificationService;
            _commonService = commonService;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(StaffRequestDto dto)
        {
            // ---------- 1. Permission & business validation ----------
            ValidateOrganisation(dto);
            //ApplyStaffTypeAndOrganisationRules(dto,)

            // ---------- 2. Duplicate login check ----------
            //if (await _context.UsersLogins
            //    .AnyAsync(x => x.LoginName == dto.LoginName))
            //{
            //    throw new CustomException("User with same login name already exists");
            //}

            // ---------- 3. Begin transaction ----------
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // ---------- 4. Create HrStaffMaster ----------
                var staff = new HrStaffMaster
                {
                    StaffType = dto.StaffType,
                    //OrganisationId = dto.OrganisationId,

                    // organisation mapping (NEW)
                    UniversityId = dto.StaffType == (long)StaffType.University? dto.UniversityId : null,

                    SchoolId = dto.StaffType == (long)StaffType.School? dto.SchoolId: null,

                    NgoId = dto.StaffType == (long)StaffType.Ngo? dto.NgoId: null,

                    StaffSalutation = dto.StaffSalutation,
                    StaffFirstName = dto.StaffFirstName,
                    StaffLastName = dto.StaffLastName,
                    Gender = dto.Gender,

                    PermAddress = dto.PermAddress,
                    PermCity = dto.PermCity,
                    PermZipCode = dto.PermZipCode,
                    PermState = dto.PermState,
                    PremCountry = dto.PermCountry,

                    OfficeEmail = dto.OfficeEmail,
                    PersonelEmail = dto.PersonelEmail,
                    MobileNo = dto.MobileNo,

                    Remarks = dto.Remarks,
                    IsActive = true,

                    CreatedBy = dto.CreatedBy,
                    CreatedDate = dto.CreatedDate
                };

                _context.HrStaffMasters.Add(staff);
                await _context.SaveChangesAsync();

                var generatedPassword = HelperMethods.GeneratePassword();
                var loginName = HelperMethods.GenerateUsername(dto.StaffType, staff.StaffId);

                // ---------- 5. Create UsersLogin ----------
                var usersLogin = new UsersLogin
                {
                    StaffId = staff.StaffId,
                    LoginName = loginName,

                    //LoginName = !string.IsNullOrWhiteSpace(dto.LoginName)
                    //? dto.LoginName : $"{staff.StaffFirstName}{staff.StaffLastName}",

                    // Password & OTP are NULL initially
                    // Password = password,
                    TempPassword = null,
                    TempPassDateTime = null,

                    ForgotEmail = dto.OfficeEmail,
                    IsActive = true,
                    Language = dto.Language,

                    CreatedBy = dto.CreatedBy,
                    CreatedDate = dto.CreatedDate
                };

                // hash AFTER object creation
                usersLogin.Password = HelperMethods.HashPassword(usersLogin, generatedPassword);

                _context.UsersLogins.Add(usersLogin);
                await _context.SaveChangesAsync();

                // ---------- 6. Commit ----------
                await transaction.CommitAsync();

                string organizationName = staff.StaffType switch
                {
                    (long)StaffType.University => await _context.UnUniversityRegistrations
                        .Where(x => x.RegistrationId == staff.UniversityId)
                        .Select(x => x.UniversityName)
                        .FirstOrDefaultAsync() ?? "",

                    (long)StaffType.School => await _context.KfSchools
                        .Where(x => x.SchoolId == staff.SchoolId)
                        .Select(x => x.SchoolName)
                        .FirstOrDefaultAsync() ?? "",

                    (long)StaffType.Ngo => "NGO Administration",
                    (long)StaffType.SuperAdmin => "System Administration",
                    (long)StaffType.Marketing => "Marketing",
                    _ => string.Empty
                };

                string fullName = $"{staff.StaffSalutation} {staff.StaffFirstName} {staff.StaffLastName}"
                                 .Trim();

                await _notificationService.SendNewUserAccountAsync(
                    usersLogin.ForgotEmail,
                    usersLogin.LoginName,
                    fullName,
                    organizationName,
                    generatedPassword
                );

                return staff.StaffId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> UpdateAsync(StaffRequestDto dto)
        {
            // ---------- 1. Business validation ----------
            ValidateOrganisation(dto);


            // ---------- 2. Fetch existing staff ----------
            var staff = await _context.HrStaffMasters
                .FirstOrDefaultAsync(x => x.StaffId == dto.StaffId);

            if (staff == null)
                throw new CustomException("Staff not found");

            // ---------- 3. Duplicate login check (exclude self) ----------
            if (!string.IsNullOrWhiteSpace(dto.LoginName))
            {
                var existingLogin = await _context.UsersLogins
                    .FirstOrDefaultAsync(x => x.StaffId == dto.StaffId);

                if (existingLogin != null &&
                    await _context.UsersLogins.AnyAsync(x =>
                        x.LoginName == dto.LoginName &&
                        x.StaffId != dto.StaffId))
                {
                    throw new CustomException("User with same login name already exists");
                }
            }

            // ---------- 4. Begin transaction ----------
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // ---------- 5. Update HrStaffMaster ----------
                staff.StaffType = dto.StaffType;

                // reset all organisation mappings first
                staff.UniversityId = null;
                staff.SchoolId = null;
                staff.NgoId = null;

                // set only the relevant one
                if (dto.StaffType == (long)StaffType.University)
                    staff.UniversityId = dto.UniversityId;

                else if (dto.StaffType == (long)StaffType.School)
                    staff.SchoolId = dto.SchoolId;

                else if (dto.StaffType == (long)StaffType.Ngo)
                    staff.NgoId = dto.NgoId;

                staff.StaffSalutation = dto.StaffSalutation;
                staff.StaffFirstName = dto.StaffFirstName;
                staff.StaffLastName = dto.StaffLastName;
                staff.Gender = dto.Gender;

                staff.PermAddress = dto.PermAddress;
                staff.PermCity = dto.PermCity;
                staff.PermZipCode = dto.PermZipCode;
                staff.PermState = dto.PermState;
                staff.PremCountry = dto.PermCountry;

                staff.OfficeEmail = dto.OfficeEmail;
                staff.PersonelEmail = dto.PersonelEmail;
                staff.MobileNo = dto.MobileNo;

                staff.Remarks = dto.Remarks;
                staff.IsActive = dto.IsActive;

                _context.HrStaffMasters.Update(staff);
                await _context.SaveChangesAsync();

                // ---------- 6. Update UsersLogin ----------
                var usersLogin = await _context.UsersLogins
                    .FirstOrDefaultAsync(x => x.StaffId == dto.StaffId);

                if (usersLogin == null)
                    throw new CustomException("User login not found");

                // update from login name api
                //usersLogin.LoginName = dto.LoginName;
                usersLogin.ForgotEmail = dto.OfficeEmail;
                usersLogin.Language = dto.Language;
                usersLogin.IsActive = dto.IsActive;

                _context.UsersLogins.Update(usersLogin);
                await _context.SaveChangesAsync();

                // ---------- 6. Commit ----------
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }



        // ---------------- DELETE (Soft) ----------------
        public async Task<bool> DeleteAsync(long staffId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var staff = await _context.HrStaffMasters
                    .FirstOrDefaultAsync(x => x.StaffId == staffId);

                if (staff == null)
                    throw new CustomException("Staff not found");

                var usersLogin = await _context.UsersLogins
                    .FirstOrDefaultAsync(x => x.StaffId == staffId);

                if (usersLogin == null)
                    throw new CustomException("User login not found");

                // Soft delete
                staff.IsActive = false;
                staff.Remarks += "[Deleted]";
                usersLogin.IsActive = false;

                _context.HrStaffMasters.Update(staff);
                _context.UsersLogins.Update(usersLogin);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        // ---------------- GET BY ID ----------------
        public async Task<StaffRequestDto?> GetByIdAsync(long id)
        {
            var staff = await _context.HrStaffMasters
                .AsNoTracking()
                .Include(x => x.University)
                .Include(x => x.School)
                .Include(x => x.UsersLogins)
                .Where(x => x.StaffId == id && x.IsActive)
                .Select(x => new StaffRequestDto
                {
                    StaffId = x.StaffId,

                    StaffType = x.StaffType,
                    StaffTypeName = x.StaffTypeNavigation == null
                                    ? null : x.StaffTypeNavigation.ModuleName,

                    UniversityId = x.UniversityId,
                    SchoolId = x.SchoolId,
                    NgoId = x.NgoId,

                    // organisation name (CLEAN)
                    OrganisationName = x.StaffType == (long)StaffType.University ? x.University!.UniversityName :
                                       x.StaffType == (long)StaffType.School ? x.School!.SchoolName :
                                       x.StaffType == (long)StaffType.SuperAdmin ? "Super Admin" :
                                       x.StaffType == (long)StaffType.Ngo ? "NGO Admin" :
                                       x.StaffType == (long)StaffType.Marketing ? "Marketing" :
                                       null,

                    StaffSalutation = x.StaffSalutation,
                    StaffFirstName = x.StaffFirstName,
                    StaffLastName = x.StaffLastName,
                    Gender = x.Gender,

                    PermAddress = x.PermAddress,
                    PermCity = x.PermCity,
                    PermZipCode = x.PermZipCode,
                    PermState = x.PermState,
                    PermCountry = x.PremCountry,
                    Photo = _commonService.GetProfileImageUrl(x.Photo),

                    OfficeEmail = x.OfficeEmail,
                    PersonelEmail = x.PersonelEmail,
                    MobileNo = x.MobileNo,

                    Remarks = x.Remarks,
                    IsActive = x.IsActive,

                    LoginName = x.UsersLogins.Select(u => u.LoginName).FirstOrDefault(),

                    Language = x.UsersLogins.Select(u => u.Language).FirstOrDefault(),

                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate
                })
                .FirstOrDefaultAsync();

            return staff;
        }



        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<StaffRequestDto>> GetByFilterAsync(StaffFilterDto filter, LoggedInUserDto currentUser)
        {
            var query = _context.HrStaffMasters
                .AsNoTracking()
                .Include(x => x.University)
                .Include(x => x.School)
                .Include(x => x.UsersLogins)
                .AsQueryable();

            // ---------- DATA SCOPE FILTER ----------
            if (currentUser.StaffType != StaffType.SuperAdmin)
            {
                if (currentUser.StaffType == (StaffType.University))
                {
                    query = query.Where(x => x.UniversityId == currentUser.UniversityId);
                }
                else if (currentUser.StaffType == StaffType.School)
                {
                    query = query.Where(x => x.SchoolId == currentUser.SchoolId);
                }
                else if (currentUser.StaffType == StaffType.Ngo)
                {
                    query = query.Where(x => x.NgoId == currentUser.NgoId);
                }
            }


            // Staff filter
            if (filter.StaffType.HasValue)
            {
                query = query.Where(x => x.StaffType == filter.StaffType.Value);
            }

            // organisation menu filter
            if(filter.OrganisationId.HasValue)
            {
                query = query.Where(x =>
                    (x.StaffType == (long)StaffType.University && x.UniversityId == filter.OrganisationId) ||
                    (x.StaffType == (long)StaffType.School && x.SchoolId == filter.OrganisationId) ||
                    (x.StaffType == (long)StaffType.Ngo && x.NgoId == filter.OrganisationId)
                );
            }

            // active filter
            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
            }

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();

                query = query.Where(x =>
                    x.StaffFirstName.ToLower().Contains(search) ||
                    x.StaffLastName.ToLower().Contains(search) ||
                    x.OfficeEmail.ToLower().Contains(search) ||
                    (x.MobileNo != null && x.MobileNo.ToLower().Contains(search)) ||
                    x.UsersLogins.Any(u => u.LoginName.ToLower().Contains(search))
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.StaffId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new StaffRequestDto
                {
                    StaffId = x.StaffId,

                    StaffType = x.StaffType,
                    StaffTypeName = x.StaffTypeNavigation == null
                                    ? null : x.StaffTypeNavigation.ModuleName,

                    UniversityId = x.UniversityId,
                    SchoolId = x.SchoolId,
                    NgoId = x.NgoId,

                    // organisation name (CLEAN)
                    OrganisationName = x.StaffType == (long)StaffType.University ? x.University!.UniversityName :
                                       x.StaffType == (long)StaffType.School ? x.School!.SchoolName :
                                       x.StaffType == (long)StaffType.SuperAdmin ? "Super Admin" :
                                       x.StaffType == (long)StaffType.Ngo ? "NGO Admin" :
                                       x.StaffType == (long)StaffType.Marketing ? "Marketing" :
                                       null,

                    StaffSalutation = x.StaffSalutation,
                    StaffFirstName = x.StaffFirstName,
                    StaffLastName = x.StaffLastName,
                    Gender = x.Gender,

                    PermAddress = x.PermAddress,
                    PermCity = x.PermCity,
                    PermZipCode = x.PermZipCode,
                    PermState = x.PermState,
                    PermCountry = x.PremCountry,
                    Photo = _commonService.GetProfileImageUrl(x.Photo),
           
                    OfficeEmail = x.OfficeEmail,
                    PersonelEmail = x.PersonelEmail,
                    MobileNo = x.MobileNo,

                    Remarks = x.Remarks,
                    IsActive = x.IsActive,

                    LoginName = x.UsersLogins.Select(u => u.LoginName).FirstOrDefault(),

                    Language = x.UsersLogins.Select(u => u.Language).FirstOrDefault(),

                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            return new PagedResultDto<StaffRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }




        private static void ValidateOrganisation(StaffRequestDto dto)
        {
            var count =
                (dto.UniversityId.HasValue ? 1 : 0) +
                (dto.SchoolId.HasValue ? 1 : 0) +
                (dto.NgoId.HasValue ? 1 : 0);

            if (count > 1)
                throw new CustomException("Only one organisation can be assigned to staff.");

            if (dto.StaffType == (long)StaffType.University && dto.UniversityId == null)
                throw new CustomException("UniversityId is required for University staff.");

            if (dto.StaffType == (long)StaffType.School && dto.SchoolId == null)
                throw new CustomException("SchoolId is required for School staff.");

            if (dto.StaffType == (long)StaffType.Ngo && dto.NgoId == null)
                throw new CustomException("NgoId is required for NGO staff.");

            if (dto.StaffType == (long)StaffType.SuperAdmin && count > 0)
                throw new CustomException("SuperAdmin cannot be assigned to any organisation.");
        }


        private void ApplyStaffTypeAndOrganisationRules(StaffRequestDto dto, LoggedInUserDto currentUser)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            switch (currentUser.StaffType)
            {
                case StaffType.SuperAdmin:
                    // SuperAdmin can assign any staff type
                    // BUT organisation mapping must still be consistent
                    NormalizeOrganisationMapping(dto);
                    break;

                case StaffType.Ngo:
                    if (dto.StaffType != (long)StaffType.Ngo)
                        throw new CustomException("NGO can create/update only NGO staff");

                    dto.StaffType = (long)StaffType.Ngo;
                    dto.UniversityId = null;
                    dto.SchoolId = null;
                    dto.NgoId = currentUser.NgoId;
                    break;

                case StaffType.School:
                    dto.StaffType = (long)StaffType.School;
                    dto.UniversityId = null;
                    dto.SchoolId = currentUser.SchoolId;
                    dto.NgoId = null;
                    break;

                case StaffType.University:
                    dto.StaffType = (long)StaffType.University;
                    dto.UniversityId = currentUser.UniversityId;
                    dto.SchoolId = null;
                    dto.NgoId = null;
                    break;

                default:
                    throw new CustomException("Invalid staff type");
            }
        }


        private void NormalizeOrganisationMapping(StaffRequestDto dto)
        {
            switch (dto.StaffType)
            {
                case (long)StaffType.SuperAdmin:
                    dto.UniversityId = null;
                    dto.SchoolId = null;
                    dto.NgoId = null;
                    break;

                case (long)StaffType.University:
                    dto.SchoolId = null;
                    dto.NgoId = null;
                    break;

                case (long)StaffType.School:
                    dto.UniversityId = null;
                    dto.NgoId = null;
                    break;

                case (long)StaffType.Ngo:
                    dto.UniversityId = null;
                    dto.SchoolId = null;
                    break;

                default:
                    throw new CustomException("Invalid staff type");
            }
        }


    }
}
