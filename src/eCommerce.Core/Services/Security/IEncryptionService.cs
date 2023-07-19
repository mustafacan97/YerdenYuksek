namespace eCommerce.Core.Services.Security;

public interface IEncryptionService
{
    //string CreateHash(byte[] data, string hashAlgorithm, int trimByteCount = 0);

    //string CreatePasswordHash(string password, string saltkey, string passwordFormat);

    string EncryptTextWithDefaultKey(string plainText);

    string DecryptTextWithDefaultKey(string cipherText);
}
