using eCommerce.Core.Configuration;

namespace eCommerce.Core.Entities.Configuration.CustomSettings;

public class MessageTemplatesSettings : ISettings
{
    public bool CaseInvariantReplacement { get; set; }
}
