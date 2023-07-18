using eCommerce.Core.Entities.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Logging;

public sealed class ActivityLogTypeBuilder : IEntityTypeConfiguration<ActivityLogType>
{
    public void Configure(EntityTypeBuilder<ActivityLogType> builder)
    {
        builder.ToTable("ActivityLogType");

        builder.HasKey(x => x.Id);

        builder.Property(q => q.SystemKeyword)
            .HasMaxLength(128);

        builder.Property(q => q.Name)
            .HasMaxLength(256);

        builder.HasMany(q => q.ActivityLogs)
            .WithOne(q => q.ActivityLogType)
            .HasForeignKey(q => q.ActivityLogTypeId)
            .IsRequired();
    }
}
