using eCommerce.Core.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Orders;

public sealed class ShoppingCartItemBuilder : IEntityTypeConfiguration<ShoppingCartItem>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        builder.ToTable("ShoppingCartItem");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.CreatedOnUtc)
            .HasPrecision(6);

        builder.HasOne(q => q.Product)
            .WithOne()
            .HasForeignKey<ShoppingCartItem>(q => q.ProductId)
            .IsRequired();
    }

    #endregion
}
