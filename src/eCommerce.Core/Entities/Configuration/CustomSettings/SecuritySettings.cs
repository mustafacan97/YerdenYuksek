using eCommerce.Core.Interfaces;

namespace eCommerce.Core.Entities.Configuration.CustomSettings;

public class SecuritySettings : ISettings
{
    public string EncryptionKey { get; set; }
}