using eCommerce.Core.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Orders;

public class ReturnRequestBuilder : IEntityTypeConfiguration<ReturnRequest>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ReturnRequest> builder)
    {
        builder.ToTable("ReturnRequest");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.CreatedOnUtc)
            .HasPrecision(6);

        builder.HasOne(x => x.OrderItem)
            .WithOne()
            .HasForeignKey<ReturnRequest>(x => x.OrderItemId)
            .IsRequired();
    }

    #endregion
}
