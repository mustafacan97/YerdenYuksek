using eCommerce.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Localization;

public partial class LocalizedProperty : BaseEntity
{
    public Guid EntityId { get; set; }

    public Guid LanguageId { get; set; }

    public string LocaleKeyGroup { get; set; }

    public string LocaleKey { get; set; }

    public string LocaleValue { get; set; }
}
