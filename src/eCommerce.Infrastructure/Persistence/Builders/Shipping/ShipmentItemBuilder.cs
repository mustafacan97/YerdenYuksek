using eCommerce.Core.Entities.Shipping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Shipping;

public class ShipmentItemBuilder : IEntityTypeConfiguration<ShipmentItem>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ShipmentItem> builder)
    {
        builder.ToTable("ShipmentItem");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.OrderItem)
            .WithOne()
            .HasForeignKey<ShipmentItem>(x => x.OrderItemId)
            .IsRequired();
    }

    #endregion
}
