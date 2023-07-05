using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel;
using YerdenYuksek.Core;
using YerdenYuksek.Core.Domain.Configuration;
using YerdenYuksek.Core.Domain.Customers;
using YerdenYuksek.Core.Domain.Messages;
using YerdenYuksek.Core.Domain.Security;

namespace eCommerce.Framework.Persistence.Builders;

public sealed class SettingBuilder : IEntityTypeConfiguration<Setting>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("Setting");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Value)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasData(SeedCustomerSettings());
        builder.HasData(SeedSecuritySettings());
        builder.HasData(SeedMessageSettings());
        builder.HasData(SeedEmailAccountSettings());
    }

    #endregion

    #region Methods

    private static IList<Setting> SeedCustomerSettings()
    {
        var customerSettings = new CustomerSettings
        {
            UsernamesEnabled = false,
            CheckUsernameAvailabilityEnabled = false,
            AllowUsersToChangeUsernames = false,
            DefaultPasswordFormat = PasswordFormat.Hashed,
            HashedPasswordFormat = "SHA512",
            PasswordMinLength = 6,
            PasswordMaxLength = 64,
            PasswordRequireDigit = false,
            PasswordRequireLowercase = false,
            PasswordRequireNonAlphanumeric = false,
            PasswordRequireUppercase = false,
            UnduplicatedPasswordsNumber = 4,
            PasswordRecoveryLinkDaysValid = 7,
            PasswordLifetime = 90,
            FailedPasswordAllowedAttempts = 0,
            FailedPasswordLockoutMinutes = 30,
            AllowCustomersToUploadAvatars = false,
            AvatarMaximumSizeBytes = 20000,
            DefaultAvatarEnabled = true,
            ShowCustomersLocation = false,
            ShowCustomersJoinDate = false,
            AllowViewingProfiles = false,
            NotifyNewCustomerRegistration = false,
            HideDownloadableProductsTab = false,
            HideBackInStockSubscriptionsTab = false,
            DownloadableProductsValidateUser = false,
            FirstNameEnabled = true,
            FirstNameRequired = true,
            LastNameEnabled = true,
            LastNameRequired = true,
            GenderEnabled = true,
            DateOfBirthEnabled = false,
            DateOfBirthRequired = false,
            DateOfBirthMinimumAge = null,
            StreetAddressEnabled = false,
            StreetAddress2Enabled = false,
            ZipCodeEnabled = false,
            CityEnabled = false,
            CountyEnabled = false,
            CountyRequired = false,
            CountryEnabled = false,
            CountryRequired = false,
            PhoneEnabled = false,
            AcceptPrivacyPolicyEnabled = false,
            NewsletterEnabled = true,
            NewsletterTickedByDefault = true,
            HideNewsletterBlock = false,
            NewsletterBlockAllowToUnsubscribe = false,
            OnlineCustomerMinutes = 20,
            StoreLastVisitedPage = false,
            StoreIpAddresses = true,
            LastActivityMinutes = 15,
            SuffixDeletedCustomers = false,
            EnteringEmailTwice = false,
            RequireRegistrationForDownloadableProducts = false,
            AllowCustomersToCheckGiftCardBalance = false,
            DeleteGuestTaskOlderThanMinutes = 1440,
            PhoneNumberValidationEnabled = false,
            PhoneNumberValidationUseRegex = false,
            PhoneNumberValidationRule = "^[0-9]{1,14}?$"
        };

        var settings = new List<Setting>() { };
        var properties = typeof(CustomerSettings).GetProperties();

        foreach (var prop in properties)
        {
            if (!prop.CanRead || !prop.CanWrite)
            {
                continue;
            }

            if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
            {
                continue;
            }

            var key = typeof(CustomerSettings).Name + "." + prop.Name;
            var value = prop.GetValue(customerSettings, null);
            var setting = new Setting()
            {
                Id = Guid.NewGuid(),
                Name = key.Trim().ToLowerInvariant(),
                Value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertToInvariantString(value) ?? string.Empty
            };

            settings.Add(setting);            
        }

        return settings;
    }

    private static IList<Setting> SeedSecuritySettings()
    {
        var securitySettings = new SecuritySettings
        {
            EncryptionKey = CommonHelper.GenerateRandomDigitCode(16),
            AdminAreaAllowedIpAddresses = null,
            HoneypotEnabled = false,
            HoneypotInputName = "hpinput",
            AllowNonAsciiCharactersInHeaders = true,
            UseAesEncryptionAlgorithm = true,
            AllowStoreOwnerExportImportCustomersWithHashedPassword = true
        };

        var settings = new List<Setting>() { };
        var properties = typeof(SecuritySettings).GetProperties();

        foreach (var prop in properties)
        {
            if (!prop.CanRead || !prop.CanWrite)
            {
                continue;
            }

            if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
            {
                continue;
            }

            var key = typeof(SecuritySettings).Name + "." + prop.Name;
            var value = prop.GetValue(securitySettings, null);
            var setting = new Setting()
            {
                Id = Guid.NewGuid(),
                Name = key.Trim().ToLowerInvariant(),
                Value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertToInvariantString(value) ?? string.Empty
            };

            settings.Add(setting);
        }

        return settings;
    }

    private static IList<Setting> SeedMessageSettings()
    {
        var messageSettings = new MessageSettings { 
            UsePopupNotifications = false,
            UseDefaultEmailAccountForSendStoreOwnerEmails = false
        };

        var settings = new List<Setting>() { };
        var properties = typeof(MessageSettings).GetProperties();

        foreach (var prop in properties)
        {
            if (!prop.CanRead || !prop.CanWrite)
            {
                continue;
            }

            if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
            {
                continue;
            }

            var key = typeof(MessageSettings).Name + "." + prop.Name;
            var value = prop.GetValue(messageSettings, null);
            var setting = new Setting()
            {
                Id = Guid.NewGuid(),
                Name = key.Trim().ToLowerInvariant(),
                Value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertToInvariantString(value) ?? string.Empty
            };

            settings.Add(setting);
        }

        return settings;
    }

    private static IList<Setting> SeedEmailAccountSettings()
    {
        var emailAccountSettings = new EmailAccountSettings();
        var settings = new List<Setting>() { };
        var properties = typeof(EmailAccountSettings).GetProperties();

        foreach (var prop in properties)
        {
            if (!prop.CanRead || !prop.CanWrite)
            {
                continue;
            }

            if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
            {
                continue;
            }

            var key = typeof(EmailAccountSettings).Name + "." + prop.Name;
            var value = prop.GetValue(emailAccountSettings, null);
            var setting = new Setting()
            {
                Id = Guid.NewGuid(),
                Name = key.Trim().ToLowerInvariant(),
                Value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertToInvariantString(value) ?? string.Empty
            };

            settings.Add(setting);
        }

        return settings;
    }

    #endregion
}
