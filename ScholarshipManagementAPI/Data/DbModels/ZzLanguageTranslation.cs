using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzLanguageTranslation
{
    public long TranslationId { get; set; }

    public long LabelId { get; set; }

    public long LanguageId { get; set; }

    public string LabelValue { get; set; } = null!;

    public bool IsActive { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual KfUsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ZzLabel Label { get; set; } = null!;

    public virtual ZzLanguage Language { get; set; } = null!;

    public virtual KfUsersLogin? UpdatedByNavigation { get; set; }
}
