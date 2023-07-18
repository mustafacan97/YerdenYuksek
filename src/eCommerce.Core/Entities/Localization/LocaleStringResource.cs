using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Localization;

public class LocaleStringResource : BaseEntity
{
    public string ResourceName { get; set; }

    public string ResourceValue { get; set; }

    public Guid LanguageId { get; set; }
}
