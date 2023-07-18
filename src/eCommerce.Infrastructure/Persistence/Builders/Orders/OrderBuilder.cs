using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using eCommerce.Core.Entities.Orders;

namespace eCommerce.Infrastructure.Persistence.Builders.Orders;

public class OrderBuilder : IEntityTypeConfiguration<Order>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.CurrencyCode)
            .HasMaxLength(4);

        builder.Property(q => q.CurrencyRate)
            .HasPrecision(18, 4);

        builder.Property(q => q.SubtotalInclTax)
            .HasPrecision(18, 4);

        builder.Property(q => q.SubtotalExclTax)
            .HasPrecision(18, 4);

        builder.Property(q => q.ShippingInclTax)
            .HasPrecision(18, 4);

        builder.Property(q => q.ShippingExclTax)
            .HasPrecision(18, 4);

        builder.Property(q => q.PaymentMethodAdditionalFeeInclTax)
            .HasPrecision(18, 4);

        builder.Property(q => q.PaymentMethodAdditionalFeeExclTax)
            .HasPrecision(18, 4);

        builder.Property(q => q.TaxRates)
            .HasMaxLength(128);

        builder.Property(q => q.Tax)
            .HasPrecision(18, 4);

        builder.Property(q => q.Discount)
            .HasPrecision(18, 4);

        builder.Property(q => q.Total)
            .HasPrecision(18, 4);

        builder.Property(q => q.RefundedAmount)
            .HasPrecision(18, 4);

        builder.Property(q => q.IpAddress)
            .HasMaxLength(64);

        builder.Property(q => q.PaidDateUtc)
            .HasPrecision(6);

        builder.Property(q => q.CreatedOnUtc)
            .HasPrecision(6);

        builder.HasOne(q => q.OrderNote)
            .WithOne()
            .HasForeignKey<OrderNote>(q => q.OrderId)
            .IsRequired();

        builder.HasMany(q => q.OrderItems)
            .WithOne()
            .HasForeignKey(q => q.OrderId)
            .IsRequired();

        builder.HasMany(q => q.Shipments)
            .WithOne(q => q.Order)
            .HasForeignKey(q => q.OrderId)
            .IsRequired();
    }

    #endregion
}
