﻿using System.Security.Cryptography;

namespace eCommerce.Core.Helpers;

public static class HashHelper
{
    #region Public Methods

    public static string CreateHash(byte[] data, string hashAlgorithm, int trimByteCount = 0)
    {
        if (string.IsNullOrEmpty(hashAlgorithm))
        {
            throw new ArgumentNullException(nameof(hashAlgorithm));
        }

        var algorithm = CryptoConfig.CreateFromName(hashAlgorithm) as HashAlgorithm;
        if (algorithm is null)
        {
            throw new ArgumentException("Unrecognized hash name");
        }

        if (trimByteCount > 0 && data.Length > trimByteCount)
        {
            var newData = new byte[trimByteCount];
            Array.Copy(data, newData, trimByteCount);

            return BitConverter.ToString(algorithm.ComputeHash(newData)).Replace("-", string.Empty);
        }

        return BitConverter.ToString(algorithm.ComputeHash(data)).Replace("-", string.Empty);
    }

    #endregion
}
