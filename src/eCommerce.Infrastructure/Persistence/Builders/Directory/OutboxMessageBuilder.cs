using eCommerce.Core.Entities.Directory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Directory;

public class OutboxMessageBuilder : IEntityTypeConfiguration<OutboxMessage>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessage");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .HasMaxLength(256);

        builder.Property(x => x.Error)
            .HasMaxLength(512);

        builder.Property(x => x.CreatedOnUtc)
            .HasPrecision(6);

        builder.Property(x => x.ProcessedOnUtc)
            .HasPrecision(6);
    }

    #endregion
}
