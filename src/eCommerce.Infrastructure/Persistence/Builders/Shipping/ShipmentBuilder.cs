using eCommerce.Core.Entities.Shipping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Shipping;

public class ShipmentBuilder : IEntityTypeConfiguration<Shipment>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("Shipment");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TrackingNumber)
            .HasMaxLength(256);

        builder.Property(x => x.TotalWeight)
            .HasPrecision(18, 4);

        builder.Property(x => x.ShippedDateUtc)
            .HasPrecision(6);

        builder.Property(x => x.DeliveryDateUtc)
            .HasPrecision(6);

        builder.Property(x => x.CreatedOnUtc)
            .HasPrecision(6);

        builder.HasMany(x => x.ShipmentItems)
            .WithOne()
            .HasForeignKey(x => x.ShipmentId)
            .IsRequired();
    }

    #endregion
}
