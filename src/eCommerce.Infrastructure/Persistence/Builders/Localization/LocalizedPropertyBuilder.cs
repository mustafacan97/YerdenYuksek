using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using eCommerce.Core.Entities.Localization;

namespace eCommerce.Infrastructure.Persistence.Builders.Localization;

public sealed class LocalizedPropertyBuilder : IEntityTypeConfiguration<LocalizedProperty>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<LocalizedProperty> builder)
    {
        builder.ToTable("LocalizedProperty");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LocaleKeyGroup)
            .HasMaxLength(256);

        builder.Property(x => x.LocaleKey)
            .HasMaxLength(256);
    }

    #endregion
}