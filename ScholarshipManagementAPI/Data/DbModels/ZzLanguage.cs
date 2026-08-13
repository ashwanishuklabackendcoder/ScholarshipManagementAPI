using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzLanguage
{
    public long LanguageId { get; set; }

    public string LanguageName { get; set; } = null!;

    public string LanguageCode { get; set; } = null!;

    public string CultureCode { get; set; } = null!;

    public bool IsRtl { get; set; }

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual KfUsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual KfUsersLogin? UpdatedByNavigation { get; set; }

    public virtual ICollection<ZzLanguageTranslation> ZzLanguageTranslations { get; set; } = new List<ZzLanguageTranslation>();
}
