using eCommerce.Core.Configuration;

namespace eCommerce.Core.Domain.Configuration.CustomSettings;

public class SecuritySettings : ISettings
{
    public string EncryptionKey { get; set; }
}