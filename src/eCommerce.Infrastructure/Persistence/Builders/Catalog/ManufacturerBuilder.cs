using eCommerce.Core.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Catalog;

public class ManufacturerBuilder : IEntityTypeConfiguration<Manufacturer>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Manufacturer> builder)
    {
        builder.ToTable("Manufacturer");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Name)
            .HasMaxLength(256);

        builder.Property(q => q.PageSizeOptions)
            .HasMaxLength(128);

        builder.Property(q => q.CreatedOnUtc)
            .HasPrecision(6);

        builder.Property(q => q.Active)
            .HasDefaultValue(true);

        builder.HasMany(q => q.Products)
            .WithOne()
            .HasForeignKey(q => q.ManufacturerId)
            .IsRequired();
    }

    #endregion
}
