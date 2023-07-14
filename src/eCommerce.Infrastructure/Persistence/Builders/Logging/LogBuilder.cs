using eCommerce.Core.Domain.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Logging;

public sealed class LogBuilder : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.ToTable("Log");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ShortMessage)
            .HasMaxLength(256);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(64);

        builder.Property(e => e.CreatedOnUtc)
            .HasPrecision(6);
    }
}
