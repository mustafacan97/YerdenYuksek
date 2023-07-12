using YerdenYuksek.Core.Configuration;

namespace YerdenYuksek.Core.Domain.Localization;

public class LanguageSettings : ISettings
{
    public static Guid DefaultLanguageId { get; set; } = Guid.Parse("38c2a49e-9f22-455e-86fd-8661eb04468d");
}
