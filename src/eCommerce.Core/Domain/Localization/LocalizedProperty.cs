using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Localization;

public partial class LocalizedProperty : BaseEntity
{
    public string LocaleKeyGroup { get; set; }

    public string LocaleKey { get; set; }

    public string LocaleValue { get; set; }

    public Guid EntityId { get; set; }

    public Guid LanguageId { get; set; }    
}
