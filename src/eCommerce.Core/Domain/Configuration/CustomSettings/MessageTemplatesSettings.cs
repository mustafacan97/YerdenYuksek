using eCommerce.Core.Configuration;

namespace eCommerce.Core.Domain.Configuration.CustomSettings;

public class MessageTemplatesSettings : ISettings
{
    public bool CaseInvariantReplacement { get; set; }
}
