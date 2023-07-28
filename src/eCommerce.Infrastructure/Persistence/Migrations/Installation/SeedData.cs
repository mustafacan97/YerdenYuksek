using eCommerce.Core.Entities.Configuration.CustomSettings;
using eCommerce.Core.Entities.Configuration;
using eCommerce.Core.Entities.Directory;
using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Entities.Messages;
using eCommerce.Core.Entities.ScheduleTasks;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Shared;
using eCommerce.Infrastructure.BackgroundJobs;
using eCommerce.Infrastructure.Persistence.DataProviders;
using FluentMigrator;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Globalization;
using eCommerce.Infrastructure.Extensions;
using eCommerce.Core.Entities.Security;

namespace eCommerce.Infrastructure.Persistence.Migrations.Installation;

[Migration(2507202302, "Seed primitives datas")]
public class SeedData : ForwardOnlyMigration
{
    #region Fields

    private readonly ICustomDataProvider _customDataProvider;

    private readonly IConfiguration _configuration;

    #endregion

    #region Constructure and Destructure

    public SeedData(ICustomDataProvider customDataProvider, IConfiguration configuration)
    {
        _customDataProvider = customDataProvider;
        _configuration = configuration;
    }

    #endregion

    #region Public Methods

    public override void Up()
    {
        SeedCurrencyData();
        SeedLanguageData();
        SeedEmailAccountData();
        SeedScheduleTaskData();
        SeedEmailAccountSettings();
        SeedLocalizationSettings();
        SeedCustomerSettings();
        SeedSecuritySettings();
        SeedEmailTemplateSettings();
        SeedMessagesSettings();
        SeedCustomerRoles();
        SeedEmailTemplateData();
    }

    #endregion

    #region Methods

    private async void SeedCurrencyData()
    {
        var currency = new Currency
        {
            Id = Guid.NewGuid(),
            Name = "US Dollar",
            CurrencyCode = "USD",
            DisplayLocale = "en-US",
            CustomFormatting = string.Empty,
            Rate = 1,
            DisplayOrder = 1,
            RoundingTypeId = (int)RoundingType.Rounding001,
            CreatedOnUtc = DateTime.UtcNow,
            Active = true,
            Deleted = false
        };

        await _customDataProvider.InsertEntityAsync(currency);
    }

    private async void SeedLanguageData()
    {
        var defaultCurrency = _customDataProvider.GetTable<Currency>().First();
        var defaultCulture = new CultureInfo("en-US");
        var defaultLanguage = new Language
        {
            Id = Guid.NewGuid(),
            Name = defaultCulture.TwoLetterISOLanguageName.ToUpperInvariant(),
            LanguageCulture = defaultCulture.Name,
            UniqueSeoCode = defaultCulture.TwoLetterISOLanguageName,
            FlagImageFileName = $"{defaultCulture.Name.ToLowerInvariant()[^2..]}.png",
            DefaultCurrencyId = defaultCurrency.Id,
            IsDefaultLanguage = true,
            DisplayOrder = 1,
            CreatedOnUtc = DateTime.UtcNow,
            Active = true,
            Deleted = false,
        };

        await _customDataProvider.InsertEntityAsync(defaultLanguage);
    }

    private async void SeedEmailAccountData()
    {
        var defaultEmailPassword = _configuration.GetValue<string>("DefaultValues:EmailAccount:Password");
        var saltKey = EncryptionHelper.CreateSaltKey(12);
        var password = EncryptionHelper.EncryptText(defaultEmailPassword, saltKey);

        var defaultEmail = new EmailAccount
        {
            Id = Guid.NewGuid(),
            Email = _configuration.GetValue<string>("DefaultValues:EmailAccount:Email"),
            Host = _configuration.GetValue<string>("DefaultValues:EmailAccount:Host"),
            Port = _configuration.GetValue<int>("DefaultValues:EmailAccount:Port"),
            Username = _configuration.GetValue<string>("DefaultValues:EmailAccount:Username"),
            Password = password,
            PasswordSalt = saltKey,
            EnableSsl = _configuration.GetValue<bool>("DefaultValues:EmailAccount:EnableSsl"),
            Active = true,
            Deleted = false
        };

        await _customDataProvider.InsertEntityAsync(defaultEmail);
    }

    private async void SeedScheduleTaskData()
    {
        var tasks = new List<ScheduleTask>
        {
            new ScheduleTask
            {
                Id = Guid.NewGuid(),
                Name = "Send emails from queue",
                Seconds = 60,
                Type = typeof(QueuedMessagesSendTask).AssemblyQualifiedName!,
                Active = true
            },
            new ScheduleTask
            {
                Id = Guid.NewGuid(),
                Name = "Process outbox messages",
                Seconds = 20,
                Type = typeof(ProcessOutboxMessagesTask).AssemblyQualifiedName!,
                Active = true
            }
        };

        await _customDataProvider.BulkInsertEntitiesAsync(tasks);
    }

    private async void SeedSettingsAsync<T>(object? baseSettingObject) where T : ISettings
    {
        var settings = new List<Setting>() { };
        var properties = typeof(T).GetProperties();

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

            var key = typeof(T).Name + "." + prop.Name;
            var value = prop.GetValue(baseSettingObject, null);
            var setting = new Setting()
            {
                Id = Guid.NewGuid(),
                Name = key.Trim().ToLowerInvariant(),
                Value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertToInvariantString(value) ?? string.Empty
            };

            settings.Add(setting);
        }

        await _customDataProvider.BulkInsertEntitiesAsync(settings);
    }

    private async void SeedEmailAccountSettings()
    {
        var emailAccount = await _customDataProvider.GetTable<EmailAccount>().FirstAsync();
        var emailAccountSettings = new EmailAccountSettings()
        {
            DefaultEmailAccountId = emailAccount.Id
        };

        SeedSettingsAsync<EmailAccountSettings>(emailAccountSettings);
    }

    private async void SeedLocalizationSettings()
    {
        var language = await _customDataProvider.GetTable<Language>().FirstAsync();
        var localizationSettings = new LocalizationSettings()
        {
            DefaultLanguageId = language.Id,
            LoadAllLocaleRecordsOnStartup = true,
            LoadAllLocalizedPropertiesOnStartup = true,
        };

        SeedSettingsAsync<LocalizationSettings>(localizationSettings);
    }

    private void SeedCustomerSettings()
    {
        var customerSettings = new CustomerSettings()
        {
            HashedPasswordFormat = "SHA512",
            PasswordMinLength = 6,
            PasswordMaxLength = 64,
            PasswordRequireDigit = false,
            PasswordRequireLowercase = false,
            PasswordRequireNonAlphanumeric = false,
            PasswordRequireUppercase = false,
            FailedPasswordAllowedAttempts = 3,
            FailedPasswordLockoutMinutes = 30,
        };

       SeedSettingsAsync<CustomerSettings>(customerSettings);
    }

    private void SeedSecuritySettings()
    {
        var securitySettings = new SecuritySettings()
        {
            EncryptionKey = CommonHelper.GenerateRandomDigitCode(16),
        };

        SeedSettingsAsync<SecuritySettings>(securitySettings);
    }

    private void SeedEmailTemplateSettings()
    {
        var messageSettings = new MessageTemplatesSettings()
        {
            CaseInvariantReplacement = false,
        };

        SeedSettingsAsync<MessageTemplatesSettings>(messageSettings);
    }

    private void SeedMessagesSettings()
    {
        var messageSettings = new MessageSettings() { };

        SeedSettingsAsync<MessageSettings>(messageSettings);
    }

    private async void SeedCustomerRoles()
    {
        var defaultRoles = new List<Role>()
        {
            new Role
            {
                Name = RoleDefaults.AdministratorsRoleName,
                Active = true,
                Deleted = false,
                CreatedOnUtc = DateTime.UtcNow
            },
            new Role
            {
                Name = RoleDefaults.ForumModeratorsRoleName,
                Active = true,
                Deleted = false,
                CreatedOnUtc = DateTime.UtcNow
            },
            new Role
            {
                Name = RoleDefaults.RegisteredRoleName,
                Active = true,
                Deleted = false,
                CreatedOnUtc = DateTime.UtcNow
            },
            new Role
            {
                Name = RoleDefaults.GuestsRoleName,
                Active = true,
                Deleted = false,
                CreatedOnUtc = DateTime.UtcNow
            },
            new Role
            {
                Name = RoleDefaults.VendorsRoleName,
                Active = true,
                Deleted = false,
                CreatedOnUtc = DateTime.UtcNow
            }
        };

        await _customDataProvider.BulkInsertEntitiesAsync(defaultRoles);
    }

    private async void SeedEmailTemplateData()
    {
        var emailAccount = await _customDataProvider.GetTable<EmailAccount>().FirstAsync();
        var templates = new List<EmailTemplate>()
        {
            new EmailTemplate
            {
                Id = Guid.NewGuid(),
                Name = EmailTemplateSystemNames.CustomerWelcomeMessage,
                Bcc = null,
                Subject = "Welcome to %Store.Name%",
                Body = $"We welcome you to <a href=\"%Store.URL%\"> %Store.Name%</a>.{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}You can now take part in the various services we have to offer you. Some of these services include:{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}Permanent Cart - Any products added to your online cart remain there until you remove them, or check them out.{Environment.NewLine}<br />{Environment.NewLine}Address Book - We can now deliver your products to another address other than yours! This is perfect to send birthday gifts direct to the birthday-person themselves.{Environment.NewLine}<br />{Environment.NewLine}Order History - View your history of purchases that you have made with us.{Environment.NewLine}<br />{Environment.NewLine}Products Reviews - Share your opinions on products with our other customers.{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}For help with any of our online services, please email the store-owner: <a href=\"mailto:%Store.Email%\">%Store.Email%</a>.{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}Note: This email address was provided on our registration page. If you own the email and did not register on our site, please send an email to <a href=\"mailto:%Store.Email%\">%Store.Email%</a>.{Environment.NewLine}",
                EmailAccountId = emailAccount.Id,
                CreatedOnUtc = DateTime.UtcNow,
                Active = true,
                Deleted = false
            },
            new EmailTemplate
            {
                Id = Guid.NewGuid(),
                Name = EmailTemplateSystemNames.CustomerEmailValidationMessage,
                Bcc = null,
                Subject = "%Store.Name%. Email validation",
                Body = $"<a href=\"%Store.URL%\">%Store.Name%</a>{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}To activate your account <a href=\"%Customer.AccountActivationURL%\">click here</a>.{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}%Store.Name%{Environment.NewLine}",
                EmailAccountId = emailAccount.Id,
                CreatedOnUtc = DateTime.UtcNow,
                Active = true,
                Deleted = false
            }
        };
        

        await _customDataProvider.BulkInsertEntitiesAsync(templates);
    }

    #endregion
}
