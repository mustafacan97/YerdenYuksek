using YerdenYuksek.Core.Configuration;

namespace YerdenYuksek.Core.Domain.Security;

public class SecuritySettings : ISettings
{
    #region Public Properties

    public string EncryptionKey { get; set; }

    public List<string> AdminAreaAllowedIpAddresses { get; set; }

    public bool HoneypotEnabled { get; set; }

    public string HoneypotInputName { get; set; }

    public bool AllowNonAsciiCharactersInHeaders { get; set; }

    public bool UseAesEncryptionAlgorithm { get; set; }

    public bool AllowStoreOwnerExportImportCustomersWithHashedPassword { get; set; }

    #endregion
}