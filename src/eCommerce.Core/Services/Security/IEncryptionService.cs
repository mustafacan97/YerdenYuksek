namespace eCommerce.Core.Services.Security;

public interface IEncryptionService
{
    string EncryptTextWithDefaultKey(string plainText);

    string DecryptTextWithDefaultKey(string cipherText);
}
