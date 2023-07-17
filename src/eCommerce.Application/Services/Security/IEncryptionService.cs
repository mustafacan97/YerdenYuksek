namespace eCommerce.Application.Services.Security;

public interface IEncryptionService
{
    string EncryptText(string plainText, string encryptionPrivateKey = "");

    string DecryptText(string cipherText, string encryptionPrivateKey = "");
}
