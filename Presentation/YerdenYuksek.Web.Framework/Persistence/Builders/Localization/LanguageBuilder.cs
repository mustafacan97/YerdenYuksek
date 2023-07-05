using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using YerdenYuksek.Core.Domain.Localization;
using System.Globalization;

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
            .HasMaxLength(4);

        builder.Property(e => e.FlagImageFileName)
            .HasMaxLength(16);

        builder.HasData(SeedLanguageData());
    }

    #endregion

    #region Methods

    private static IList<Language> SeedLanguageData()
    {
        var defaultCulture = new CultureInfo("tr-TR");
        var defaultLanguage = new Language
        {
            Id = Guid.NewGuid(),
            Name = defaultCulture.TwoLetterISOLanguageName.ToUpperInvariant(),
            LanguageCulture = defaultCulture.Name,
            UniqueSeoCode = defaultCulture.TwoLetterISOLanguageName,
            FlagImageFileName = $"{defaultCulture.Name.ToLowerInvariant()[^2..]}.png",
            Rtl = defaultCulture.TextInfo.IsRightToLeft,
            IsDefaultLanguage = true,
            DisplayOrder = 1,
            Active = true,
            Deleted = false,
        };

        return new List<Language>() { defaultLanguage };
    }

    #endregion
}