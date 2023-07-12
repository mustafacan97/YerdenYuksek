using eCommerce.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Localization;

public class Language : BaseEntity
{
    #region Constructure and Destructure

    public Language()
    {
        LocaleStringResources = new HashSet<LocaleStringResource>();
    }

    #endregion

    #region Public Properties

    public string Name { get; set; }

    public string LanguageCulture { get; set; }

    public string UniqueSeoCode { get; set; }

    public string? FlagImageFileName { get; set; }

    public bool Rtl { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsDefaultLanguage { get; set; }

    public bool Active { get; set; }

    public bool Deleted { get; set; }

    public ICollection<LocaleStringResource> LocaleStringResources { get; set; }

    #endregion
}
