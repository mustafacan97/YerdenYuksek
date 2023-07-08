using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.ScheduleTasks;

namespace YerdenYuksek.Web.Framework.Persistence.Builders.ScheduleTasks;

public class ScheduleTaskBuilder : IEntityTypeConfiguration<ScheduleTask>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ScheduleTask> builder)
    {
        builder.ToTable("ScheduleTask");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(512);

        builder.Property(e => e.Type)
            .HasMaxLength(512);

        builder.Property(e => e.LastEnabledUtc)
            .HasPrecision(6);

        builder.Property(e => e.Enabled)
            .HasDefaultValue(true);

        builder.Property(e => e.LastStartUtc)
            .HasPrecision(6);

        builder.Property(e => e.LastEndUtc)
            .HasPrecision(6);

        builder.Property(e => e.LastSuccessUtc)
            .HasPrecision(6);

        builder.HasData(SeedScheduleTaskData());
    }

    #endregion

    #region Methods

    private static IList<ScheduleTask> SeedScheduleTaskData()
    {
        var tasks = new List<ScheduleTask>
        {
            new ScheduleTask
            {
                Id = Guid.NewGuid(),
                Name = "Send emails",
                Seconds = 60,
                Type = "YerdenYuksek.Services.Messages.QueuedMessagesSendTask",
                Enabled = true,
                LastEnabledUtc = DateTime.UtcNow,
                StopOnError = false
            }
        };

        return tasks;
    }

    #endregion
}
