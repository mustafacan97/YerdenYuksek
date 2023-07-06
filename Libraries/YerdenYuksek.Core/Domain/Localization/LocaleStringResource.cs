using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Localization;

public class LocaleStringResource : BaseEntity
{
    public Guid LanguageId { get; set; }

    public string ResourceName { get; set; }

    public string ResourceValue { get; set; }
}
