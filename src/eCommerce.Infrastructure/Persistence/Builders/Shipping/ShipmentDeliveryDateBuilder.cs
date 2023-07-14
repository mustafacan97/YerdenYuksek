using eCommerce.Core.Domain.Shipping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Tax;

public class ShipmentDeliveryDateBuilder : IEntityTypeConfiguration<ShipmentDeliveryDate>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ShipmentDeliveryDate> builder)
    {
        builder.ToTable("ShipmentDeliveryDate");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Name)
            .HasMaxLength(256);

        builder.HasMany(q => q.Products)
            .WithOne()
            .HasForeignKey(q => q.TaxCategoryId)
            .IsRequired();
    }

    #endregion
}
