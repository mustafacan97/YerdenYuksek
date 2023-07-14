using eCommerce.Core.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Orders;

public class OrderNoteBuilder : IEntityTypeConfiguration<OrderNote>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<OrderNote> builder)
    {
        builder.ToTable("OrderNote");

        builder.HasKey(q => q.OrderId);

        builder.Property(q => q.Note)
            .HasMaxLength(256);

        builder.Property(q => q.CreatedOnUtc)
            .HasPrecision(6);
    }

    #endregion
}
