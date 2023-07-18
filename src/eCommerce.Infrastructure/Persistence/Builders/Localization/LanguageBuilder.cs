using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using eCommerce.Core.Entities.Localization;

namespace YerdenYuksek.Web.Framework.Persistence.Builders.Localization;

public sealed class LanguageBuilder : IEntityTypeConfiguration<Language>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Language");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(64);

        builder.Property(e => e.LanguageCulture)
            .HasMaxLength(8);

        builder.Property(e => e.UniqueSeoCode)
            .HasMaxLength(2);

        builder.Property(e => e.FlagImageFileName)
            .HasMaxLength(16);

        builder.Property(x => x.CreatedOnUtc)
            .HasPrecision(6);

        builder.HasMany(q => q.LocalizedProperties)
            .WithOne()
            .HasForeignKey(q => q.LanguageId)
            .IsRequired();

        builder.HasMany(q => q.LocaleStringResources)
            .WithOne()
            .HasForeignKey(q => q.LanguageId)
            .IsRequired();

        builder.HasMany(q => q.Customers)
            .WithOne(q => q.Language)
            .HasForeignKey(q => q.LanguageId)
            .IsRequired(false);

        builder.HasData(SeedLanguageData());
    }

    #endregion

    #region Methods

    private static IList<Language> SeedLanguageData()
    {
        IConfiguration _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var defaultCulture = new CultureInfo("en-US");
        var defaultLanguage = new Language
        {
            Id = _configuration.GetValue<Guid>("DefaultValues:LanguageId"),
            Name = defaultCulture.TwoLetterISOLanguageName.ToUpperInvariant(),
            LanguageCulture = defaultCulture.Name,
            UniqueSeoCode = defaultCulture.TwoLetterISOLanguageName,
            FlagImageFileName = $"{defaultCulture.Name.ToLowerInvariant()[^2..]}.png",
            DefaultCurrencyId = _configuration.GetValue<Guid>("DefaultValues:CurrencyId"),
            IsDefaultLanguage = true,
            DisplayOrder = 1,
            CreatedOnUtc = DateTime.UtcNow,
            Active = true,
            Deleted = false,            
        };

        return new List<Language>() { defaultLanguage };
    }

    #endregion
}