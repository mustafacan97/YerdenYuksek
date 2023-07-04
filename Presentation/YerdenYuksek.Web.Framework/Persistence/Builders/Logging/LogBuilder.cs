using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Logging;

namespace YerdenYuksek.Web.Framework.Persistence.Builders.Logging;

public sealed class LogBuilder : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.ToTable("Log");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ShortMessage)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(16);

        builder.Property(x => x.LogLevelId)
            .IsRequired();

        builder.Property(x => x.EndpointUrl)
            .IsRequired();

        builder.Property(e => e.CreatedOnUtc)
            .HasPrecision(6);
    }
}
