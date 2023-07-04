using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Logging;

namespace YerdenYuksek.Web.Framework.Persistence.Builders.Logging;

public sealed class ActivityLogBuilder : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.ToTable("ActivityLog");
        
        builder.HasKey(x => x.Id);

        builder.Property(q => q.IpAddress)
            .HasMaxLength(16);

        builder.Property(e => e.CreatedOnUtc)
            .HasPrecision(6);

        builder.HasOne(q => q.ActivityLogType)
            .WithMany()
            .HasForeignKey(q => q.ActivityLogTypeId)
            .IsRequired();
    }
}
