using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Logging;

namespace YerdenYuksek.Web.Framework.Persistence.Builders.Logging;

public sealed class ActivityLogTypeBuilder : IEntityTypeConfiguration<ActivityLogType>
{
    public void Configure(EntityTypeBuilder<ActivityLogType> builder)
    {
        builder.ToTable("ActivityLogType");

        builder.HasKey(x => x.Id);

        builder.Property(q => q.SystemKeyword)
            .HasMaxLength(255);

        builder.Property(q => q.Name)
            .HasMaxLength(255);
    }
}
