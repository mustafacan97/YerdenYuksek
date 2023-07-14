using eCommerce.Core.Configuration;

namespace eCommerce.Core.Domain.Configuration.CustomSettings;

public class CustomerSettings : ISettings
{
    public string HashedPasswordFormat { get; set; }

    public int PasswordMinLength { get; set; }

    public int PasswordMaxLength { get; set; }

    public bool PasswordRequireLowercase { get; set; }

    public bool PasswordRequireUppercase { get; set; }

    public bool PasswordRequireNonAlphanumeric { get; set; }

    public bool PasswordRequireDigit { get; set; }

    public int FailedPasswordAllowedAttempts { get; set; }

    public int FailedPasswordLockoutMinutes { get; set; }
}