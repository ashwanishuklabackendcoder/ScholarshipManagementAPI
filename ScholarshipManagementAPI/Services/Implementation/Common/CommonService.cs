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