using eCommerce.Core.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Framework.Persistence.Builders.Common;

public sealed class CityBuilder : IEntityTypeConfiguration<City>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("City");

        builder.HasKey(x => x.Id);

        builder.Property(q => q.Name)
            .HasMaxLength(128);

        builder.Property(q => q.Abbreviation)
            .HasMaxLength(8);

        builder.HasMany(q => q.Addresses)
            .WithOne(q => q.City)
            .HasForeignKey(q => q.CityId)
            .IsRequired(false);
    }

    #endregion
}
