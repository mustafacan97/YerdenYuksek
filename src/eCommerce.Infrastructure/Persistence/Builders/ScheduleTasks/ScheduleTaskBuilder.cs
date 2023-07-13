using eCommerce.Core.Domain.ScheduleTasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.ScheduleTasks;

public class ScheduleTaskBuilder : IEntityTypeConfiguration<ScheduleTask>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ScheduleTask> builder)
    {
        builder.ToTable("ScheduleTask");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(256);

        builder.Property(e => e.Type)
            .HasMaxLength(512);        

        builder.Property(e => e.LastStartUtc)
            .HasPrecision(6);

        builder.Property(e => e.LastEndUtc)
            .HasPrecision(6);

        builder.Property(e => e.LastSuccessUtc)
            .HasPrecision(6);

        builder.Property(e => e.Active)
            .HasDefaultValue(true);

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
                Name = "Send emails from queue",
                Seconds = 60,
                Type = "eCommerce.Infrastructure.Persistence.Services.Public.ScheduleTasks.QueuedMessagesSendTask",
                Active = true
            }
        };

        return tasks;
    }

    #endregion
}
