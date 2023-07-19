using eCommerce.Core.Interfaces;

namespace eCommerce.Core.Entities.Configuration.CustomSettings;

public class LocalizationSettings : ISettings
{
    public Guid DefaultLanguageId { get; set; }

    public bool LoadAllLocaleRecordsOnStartup { get; set; }

    public bool LoadAllLocalizedPropertiesOnStartup { get; set; }
}