using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.Label;
using ScholarshipManagementAPI.DTOs.SuperADmin.ZzMasterDropdown;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class LabelService : ILabelService
    {
        private readonly AppDbContext _context;

        public LabelService(AppDbContext context)
        {
            _context = context;
        }


        public Task<long> CreateAsync(LabelRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(LabelRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<LabelRequestDto?> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<LabelRequestDto>> GetByFilterAsync(LabelFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public Task<LanguageLabelsDto> GetTranslations(LanguageCode language)
        {
            throw new NotImplementedException();
        }




        #region Private Methods


        // the UILanguageVersion must be incremented
        // whenever any label data changes.
        private async Task IncrementLanguageVersion()
        {
            var setting = await _context.ZzGeneralSettings
                .FirstAsync(x => x.ConfigKey == "UILanguageVersion");

            if (!int.TryParse(setting.ConfigValue, out int version))
            {
                version = 0;
            }

            setting.ConfigValue = (version + 1).ToString();

            await _context.SaveChangesAsync();
        }



        public static bool IsRtl(LanguageCode language)
        {
            return language switch
            {
                LanguageCode.Ar => true,
                _ => false
            };
        }



        #endregion


    }
}
