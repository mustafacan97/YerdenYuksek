using eCommerce.Core.Entities.Configuration.CustomSettings;
using eCommerce.Core.Services.Security;
using eCommerce.Core.Shared;

namespace eCommerce.Infrastructure.Services.Secuirty;

public class EncryptionService : IEncryptionService
{
    #region Fields

    protected readonly SecuritySettings _securitySettings;

    #endregion

    #region Constructure and Destructure

    public EncryptionService(SecuritySettings securitySettings)
    {
        _securitySettings = securitySettings;
    }

    #endregion    

    #region Public Methods

    public string EncryptTextWithDefaultKey(string plainText)
    {
        return string.IsNullOrEmpty(plainText) ?
            plainText :
            EncryptionHelper.EncryptText(plainText, _securitySettings.EncryptionKey);
    }

    public string DecryptTextWithDefaultKey(string cipherText)
    {
        return string.IsNullOrEmpty(cipherText) ?
            cipherText :
            EncryptionHelper.DecryptText(cipherText, _securitySettings.EncryptionKey);
    }

    #endregion
}