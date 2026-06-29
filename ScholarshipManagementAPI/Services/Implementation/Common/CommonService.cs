using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.HrStaff;
using ScholarshipManagementAPI.DTOs.Common.Menu;
using ScholarshipManagementAPI.DTOs.Common.Settings;
using ScholarshipManagementAPI.DTOs.SuperADmin.ZzMasterDropdown;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Services.Interface.Common;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.Common
{

    public class CommonService : ICommonService
    {
        private readonly AppDbContext _context;
        private readonly IAwsBucketService _awsBucketService;

        public CommonService(AppDbContext context, IAwsBucketService awsBucketService)
        {
            _context = context;
            _awsBucketService = awsBucketService;
        }


        public async Task<List<UsersModuleDto>> GetAllUsersModule()
        {
            return await _context.UsersModules
                .AsNoTracking()
                .OrderBy(x => x.ModuleId)
                .Select(x => new UsersModuleDto
                {
                    ModuleId = x.ModuleId,
                    ModuleName = x.ModuleName,
                    IsActive = x.IsActive
                })
                .ToListAsync();
        }


        public async Task<List<LoadMenuDto>> LoadMenusByRoleAsync(long roleId)
        {
            // Fetch role-page permissions + menu
            var roleMenus = await _context.UsersRolePages
                .AsNoTracking()
                .Include(rp => rp.MenuLink)
                .Where(rp => rp.RoleId == roleId && rp.ViewPer)
                .OrderBy(x => x.MenuLink.ParentId ?? x.MenuLink.MenuLinkId)
                .ThenBy(x => x.MenuLink.LevelNo)
                .ThenBy(x => x.MenuLink.SequenceNo)
                .Select(rp => new
                {
                    rp.MenuLink.MenuLinkId,
                    rp.MenuLink.ParentId,
                    rp.MenuLink.ModuleId,
                    rp.MenuLink.PageHeading,
                    rp.MenuLink.PagePath,
                    rp.MenuLink.ActualName,
                    rp.MenuLink.IsDashboard,
                    rp.MenuLink.SequenceNo,
                    rp.MenuLink.Icon,
                    Permissions = new MenuPermissionDto
                    {
                        CanView = rp.ViewPer,
                        CanInsert = rp.InsertPer,
                        CanUpdate = rp.UpdatePer,
                        CanDelete = rp.DeletePer
                    }
                })
                .ToListAsync();

            // Convert to dictionary
            var menuDict = roleMenus.ToDictionary(
                x => x.MenuLinkId,
                x => new LoadMenuDto
                {
                    MenuLinkId = x.MenuLinkId,
                    ModuleId = x.ModuleId,
                    PageHeading = x.PageHeading,
                    PagePath = x.PagePath,
                    ActualName = x.ActualName,
                    IsDashboard = x.IsDashboard,
                    SequenceNo = x.SequenceNo,
                    Icon = x.Icon,
                    Permissions = x.Permissions
                }
            );

            // Build hierarchy
            List<LoadMenuDto> rootMenus = new();

            foreach (var item in roleMenus)
            {
                if (item.ParentId != null && menuDict.ContainsKey(item.ParentId.Value))
                {
                    menuDict[item.ParentId.Value]
                        .SubMenus.Add(menuDict[item.MenuLinkId]);
                }
                else
                {
                    rootMenus.Add(menuDict[item.MenuLinkId]);
                }
            }

            // Sort recursively
            SortMenus(rootMenus);

            return rootMenus;
        }




        public async Task<DashboardDto> GetDashboardAsync()
        {
            var now = DateTime.Now;
            var lastMonth = now.AddMonths(-1);

            // 🔹 TOTAL COUNTS
            var nominatedCandidates = await _context.StudentReqLists.CountAsync();

            var acceptedApplications = await _context.StudentReqLists
                .CountAsync(x => x.UniAwardingstatus == (int)AwardingStatus.Awarded);

            var sponsoredCandidates = await _context.StudentReqLists
                .CountAsync(x => x.DaAdmissionStatus == (int)SponsoredStatus.Sponsored);

            var rejectedApplications = await _context.StudentReqLists
                .CountAsync(x => x.UniAwardingstatus == (int)AwardingStatus.Rejected);

            // CURRENT MONTH (status-wise)
            var nominatedThisMonth = await _context.StudentReqLists
                .CountAsync(x => x.CreatedDate.Month == now.Month && x.CreatedDate.Year == now.Year);

            var acceptedThisMonth = await _context.StudentReqLists
                .CountAsync(x => x.UniAwardingstatus == (int)AwardingStatus.Awarded
                    && x.CreatedDate.Month == now.Month && x.CreatedDate.Year == now.Year);

            var sponsoredThisMonth = await _context.StudentReqLists
                .CountAsync(x => x.DaAdmissionStatus == (int)SponsoredStatus.Sponsored
                    && x.CreatedDate.Month == now.Month && x.CreatedDate.Year == now.Year);

            var rejectedThisMonth = await _context.StudentReqLists
                .CountAsync(x => x.UniAwardingstatus == (int)AwardingStatus.Rejected
                    && x.CreatedDate.Month == now.Month && x.CreatedDate.Year == now.Year);

            // LAST MONTH (status-wise)
            var nominatedLastMonth = await _context.StudentReqLists
                .CountAsync(x => x.CreatedDate.Month == lastMonth.Month && x.CreatedDate.Year == lastMonth.Year);

            var acceptedLastMonth = await _context.StudentReqLists
                .CountAsync(x => x.UniAwardingstatus == (int)AwardingStatus.Awarded
                    && x.CreatedDate.Month == lastMonth.Month && x.CreatedDate.Year == lastMonth.Year);

            var sponsoredLastMonth = await _context.StudentReqLists
                .CountAsync(x => x.DaAdmissionStatus == (int)SponsoredStatus.Sponsored
                    && x.CreatedDate.Month == lastMonth.Month && x.CreatedDate.Year == lastMonth.Year);

            var rejectedLastMonth = await _context.StudentReqLists
                .CountAsync(x => x.UniAwardingstatus == (int)AwardingStatus.Rejected
                    && x.CreatedDate.Month == lastMonth.Month && x.CreatedDate.Year == lastMonth.Year);

            // GROWTH (per status)
            var nominatedGrowth = CalculateGrowth(nominatedThisMonth, nominatedLastMonth);
            var acceptedGrowth = CalculateGrowth(acceptedThisMonth, acceptedLastMonth);
            var sponsoredGrowth = CalculateGrowth(sponsoredThisMonth, sponsoredLastMonth);
            var rejectedGrowth = CalculateGrowth(rejectedThisMonth, rejectedLastMonth);

            // DTO
            var dto = new DashboardDto
            {
                NominatedCandidates = nominatedCandidates,
                AcceptedApplications = acceptedApplications,
                SponsoredCandidates = sponsoredCandidates,
                RejectedApplications = rejectedApplications,

                ApplicationsThisMonth = nominatedThisMonth,

                // optional global
                ApplicationsGrowthPercentage = nominatedGrowth ?? 0
            };

            // CARDS (correct growth applied)
            dto.Cards = new List<DashboardCardDto>
            {
                CreateCard("nominated", "Nominated Candidates", nominatedCandidates, nominatedGrowth),
                CreateCard("accepted", "Accepted Applications", acceptedApplications, acceptedGrowth),
                CreateCard("sponsored", "Sponsored Candidates", sponsoredCandidates, sponsoredGrowth),
                CreateCard("rejected", "Rejected Applications", rejectedApplications, rejectedGrowth)
            };

            return dto;
        }



        // Card Builder
        private DashboardCardDto CreateCard(string key, string title, int value, decimal? growth)
        {
            var (icon, color) = GetCardUI(key);

            return new DashboardCardDto
            {
                Key = key,
                Title = title,
                Value = value,
                GrowthPercentage = growth,
                Icon = icon,
                Color = color,
                Url = GetCardUrl(key)
            };
        }

        // UI Mapping
        private (string icon, string color) GetCardUI(string key)
        {
            return key switch
            {
                "nominated" => ("file", "info"),              // Blue
                "accepted" => ("check-circle", "success"),   // Green
                "sponsored" => ("school", "warning"),         // Orange
                "rejected" => ("times-circle", "danger"),    // Red
                _ => ("info", "info")
            };
        }


        private string GetCardUrl(string key)
        {
            return key switch
            {
                "nominated" => "/student-req-list",
                "accepted" => "/enrolled/students?type=accepted",
                "sponsored" => "/enrolled/students?type=sponsored",
                "rejected" => "/enrolled/students?type=rejected",
                _ => "#"
            };
        }

        // Growth Calculation
        private decimal? CalculateGrowth(int current, int previous)
        {
            if (previous == 0)
                return current > 0 ? 100 : 0;

            return ((current - previous) * 100m) / previous;
        }



        #region File Upload Methods

        public async Task<string> UploadUserProfileImageAsync(int userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is required");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid image type");

            if (file.Length > 2 * 1024 * 1024)
                throw new Exception("Image must be less than 2MB");

            var fileKey = $"users/{userId}/profile/{Guid.NewGuid()}{extension}";

            using var stream = file.OpenReadStream();

            await _awsBucketService.UploadFileAsync(
                stream,
                fileKey,
                file.ContentType
            );


            // Save fileKey in database later

            return fileKey;
        }


        public string? GetProfileImageUrl(string? fileKey)
        {
            if (string.IsNullOrEmpty(fileKey))
                return null;

            return _awsBucketService.GeneratePreSignedUrl(fileKey);
        }



        public async Task<string> UploadUserDocumentAsync(int userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is required");

            var allowedExtensions = new[] { ".pdf" };

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Only PDF documents are allowed");

            if (file.Length > 10 * 1024 * 1024)
                throw new Exception("Document size must be less than 10MB");

            var fileKey = $"users/{userId}/documents/{Guid.NewGuid()}{extension}";

            using var stream = file.OpenReadStream();

            await _awsBucketService.UploadFileAsync(
                stream,
                fileKey,
                file.ContentType
            );


            // Save fileKey in database later

            return fileKey;
        }


        public string? GetDocumentUrl(string? fileKey)
        {
            if (string.IsNullOrEmpty(fileKey))
                return null;

            return _awsBucketService.GeneratePreSignedUrl(fileKey);
        }


        #endregion





        #region Private Methods

        private void SortMenus(List<LoadMenuDto>? menus)
        {
            if (menus == null || menus.Count == 0)
                return;

            menus.Sort((a, b) => a.SequenceNo.CompareTo(b.SequenceNo));

            foreach (var menu in menus)
            {
                SortMenus(menu.SubMenus);
            }
        }

        #endregion






    }
}