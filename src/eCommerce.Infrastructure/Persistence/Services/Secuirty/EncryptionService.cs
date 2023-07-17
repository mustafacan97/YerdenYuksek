using System.Security.Cryptography;
using System.Text;
using eCommerce.Application.Services.Security;
using eCommerce.Core.Domain.Configuration.CustomSettings;
using eCommerce.Core.Helpers;

namespace eCommerce.Infrastructure.Persistence.Services.Secuirty;

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

    public static string CreateSaltKey(int size)
    {
        using var provider = RandomNumberGenerator.Create();
        var buff = new byte[size];
        provider.GetBytes(buff);

        return Convert.ToBase64String(buff);
    }

    public static string CreatePasswordHash(string password, string saltkey, string passwordFormat)
    {
        return HashHelper.CreateHash(Encoding.UTF8.GetBytes(string.Concat(password, saltkey)), passwordFormat);
    }

    public static string EncryptText(string plainText, string encryptionPrivateKey)
    {
        if (string.IsNullOrEmpty(plainText))
        {
            return plainText;
        }

        if (string.IsNullOrEmpty(encryptionPrivateKey))
        {
            return plainText;
        }

        using var provider = GetEncryptionAlgorithm(encryptionPrivateKey);
        var encryptedBinary = EncryptTextToMemory(plainText, provider);

        return Convert.ToBase64String(encryptedBinary);
    }

    public static string DecryptText(string cipherText, string encryptionPrivateKey)
    {
        if (string.IsNullOrEmpty(cipherText))
        {
            return cipherText;
        }

        if (string.IsNullOrEmpty(encryptionPrivateKey))
        {
            return cipherText;
        }

        using var provider = GetEncryptionAlgorithm(encryptionPrivateKey);
        var buffer = Convert.FromBase64String(cipherText);

        return DecryptTextFromMemory(buffer, provider);
    }

    public string EncryptTextWithDefaultKey(string plainText)
    {
        return string.IsNullOrEmpty(plainText) ? 
            plainText : 
            EncryptText(plainText, _securitySettings.EncryptionKey);
    }

    public string DecryptTextWithDefaultKey(string cipherText)
    {
        return string.IsNullOrEmpty(cipherText) ?
            cipherText :
            DecryptText(cipherText, _securitySettings.EncryptionKey);
    }

    #endregion

    #region Methods

    private static byte[] EncryptTextToMemory(string data, SymmetricAlgorithm provider)
    {
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write))
        {
            var toEncrypt = Encoding.Unicode.GetBytes(data);
            cs.Write(toEncrypt, 0, toEncrypt.Length);
            cs.FlushFinalBlock();
        }

        return ms.ToArray();
    }

    private static string DecryptTextFromMemory(byte[] data, SymmetricAlgorithm provider)
    {
        using var ms = new MemoryStream(data);
        using var cs = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Read);
        using var sr = new StreamReader(cs, Encoding.Unicode);

        return sr.ReadToEnd();
    }

    private static SymmetricAlgorithm GetEncryptionAlgorithm(string encryptionKey)
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new ArgumentNullException(nameof(encryptionKey));
        }

        SymmetricAlgorithm provider = Aes.Create();
        var vectorBlockSize = provider.BlockSize / 8;
        provider.Key = Encoding.ASCII.GetBytes(encryptionKey[0..16]);
        provider.IV = Encoding.ASCII.GetBytes(encryptionKey[^vectorBlockSize..]);

        return provider;
    }

    #endregion
}