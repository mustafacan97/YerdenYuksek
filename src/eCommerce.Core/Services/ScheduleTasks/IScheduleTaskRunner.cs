using eCommerce.Core.Entities.ScheduleTasks;

namespace eCommerce.Core.Services.ScheduleTasks;

public interface IScheduleTaskRunner
{
    Task ExecuteAsync(
        ScheduleTask scheduleTask,
        bool forceRun = false,
        bool throwException = false,
        bool ensureRunOncePerPeriod = true);
}
