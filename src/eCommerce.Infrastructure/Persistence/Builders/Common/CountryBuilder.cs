using eCommerce.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Framework.Persistence.Builders.Common;

public sealed class CountryBuilder : IEntityTypeConfiguration<Country>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Country");

        builder.HasKey(x => x.Id);

        builder.Property(q => q.Name)
            .HasMaxLength(128);

        builder.Property(q => q.TwoLetterIsoCode)
            .HasMaxLength(2);

        builder.Property(q => q.Active)
            .HasDefaultValue(true);

        builder.HasMany(q => q.Addresses)
            .WithOne(q => q.Country)
            .HasForeignKey(q => q.CountryId)
            .IsRequired(false);

        builder.HasMany(q => q.Cities)
            .WithOne()
            .HasForeignKey(q => q.CountryId)
            .IsRequired();
    }

    #endregion
}
