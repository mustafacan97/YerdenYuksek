using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using eCommerce.Core.Entities.Localization;

namespace eCommerce.Infrastructure.Persistence.Builders.Localization;

public sealed class LocaleStringResourceBuilder : IEntityTypeConfiguration<LocaleStringResource>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<LocaleStringResource> builder)
    {
        builder.ToTable("LocaleStringResource");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ResourceName)
            .HasMaxLength(256);

        builder.HasData(SeedLocaleStringResourceData());
    }

    #endregion

    #region Methods

    private static IList<LocaleStringResource> SeedLocaleStringResourceData()
    {
        var resources = new List<LocaleStringResource>() { };

        return resources;
    }

    #endregion
}