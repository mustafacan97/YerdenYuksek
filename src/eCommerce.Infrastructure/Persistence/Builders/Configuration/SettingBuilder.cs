using eCommerce.Core.Configuration;
using eCommerce.Core.Domain.Configuration;
using eCommerce.Core.Domain.Configuration.CustomSettings;
using eCommerce.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;

namespace eCommerce.Framework.Persistence.Builders;

public sealed class SettingBuilder : IEntityTypeConfiguration<Setting>
{
    #region Constructure and Destructure

    public SettingBuilder()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();
    }

    #endregion

    #region Public Properties

    public IConfiguration Configuration { get; set; }

    #endregion

    #region Public Methods

    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("Setting");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(256);

        builder.Property(x => x.Value)
            .HasMaxLength(256);

        builder.HasData(SeedCustomerSettings());
        builder.HasData(SeedSecuritySettings());
        builder.HasData(SeedEmailTemplateSettings());
        builder.HasData(SeedMessagesSettings());
        builder.HasData(SeedEmailAccountSettings());
        builder.HasData(SeedLocalizationSettings());
    }

    #endregion

    #region Methods

    private IList<Setting> SeedEmailAccountSettings()
    {
        var emailAccountSettings = new EmailAccountSettings()
        {
            DefaultEmailAccountId = Configuration.GetValue<Guid>("DefaultValues:EmailAccountId"),
        };

        return GetSettings<EmailAccountSettings>(emailAccountSettings);
    }

    private IList<Setting> SeedLocalizationSettings()
    {
        var localizationSettings = new LocalizationSettings()
        {
            DefaultLanguageId = Configuration.GetValue<Guid>("DefaultValues:LanguageId"),
            LoadAllLocaleRecordsOnStartup = true,
            LoadAllLocalizedPropertiesOnStartup = true,
        };

        return GetSettings<LocalizationSettings>(localizationSettings);
    }

    private static IList<Setting> SeedCustomerSettings()
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

        return GetSettings<CustomerSettings>(customerSettings);
    }

    private static IList<Setting> SeedSecuritySettings()
    {
        var securitySettings = new SecuritySettings() 
        {
            EncryptionKey = CommonHelper.GenerateRandomDigitCode(16),
            UseAesEncryptionAlgorithm = true,
        };
        
        return GetSettings<SecuritySettings>(securitySettings);
    }

    private static IList<Setting> SeedEmailTemplateSettings()
    {
        var messageSettings = new MessageTemplatesSettings() 
        {
            CaseInvariantReplacement = false,
        };

        return GetSettings<MessageTemplatesSettings>(messageSettings);
    }

    private static IList<Setting> SeedMessagesSettings()
    {
        var messageSettings = new MessageSettings() { };

        return GetSettings<MessageSettings>(messageSettings);
    }    

    private static IList<Setting> GetSettings<T>(object? baseSettingObject) where T : ISettings
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

        return settings;
    }

    #endregion    
}
