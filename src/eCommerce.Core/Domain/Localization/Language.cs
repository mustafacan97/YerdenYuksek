using eCommerce.Core.Domain.Directory;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Localization;

public class Language : SoftDeletedEntity
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

    public string? UniqueSeoCode { get; set; }

    public string? FlagImageFileName { get; set; }

    public Guid DefaultCurrencyId { get; set; }

    public bool IsDefaultLanguage { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public Currency DefaultCurrency { get; set; }

    public ICollection<LocaleStringResource> LocaleStringResources { get; set; }

    #endregion
}
