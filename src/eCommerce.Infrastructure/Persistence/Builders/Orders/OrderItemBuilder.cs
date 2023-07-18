using eCommerce.Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Orders;

public class OrderItemBuilder : IEntityTypeConfiguration<OrderItem>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItem");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UnitPriceInclTax)
            .HasPrecision(18, 4);

        builder.Property(x => x.UnitPriceExclTax)
            .HasPrecision(18, 4);

        builder.Property(x => x.PriceInclTax)
            .HasPrecision(18, 4);

        builder.Property(x => x.PriceExclTax)
            .HasPrecision(18, 4);

        builder.Property(x => x.OriginalProductCost)
            .HasPrecision(18, 4);
    }

    #endregion
}
