using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Logging;

namespace eCommerce.Infrastructure.Framework.Persistence.Builders.Logging;

public sealed class ActivityLogBuilder : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.ToTable("ActivityLog");
        
        builder.HasKey(x => x.Id);

        builder.Property(q => q.EntityName)
            .HasMaxLength(256);

        builder.Property(e => e.CreatedOnUtc)
            .HasPrecision(6);
    }
}
