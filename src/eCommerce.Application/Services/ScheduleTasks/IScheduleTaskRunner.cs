using eCommerce.Core.Domain.ScheduleTasks;

namespace eCommerce.Application.Services.ScheduleTasks;

public interface IScheduleTaskRunner
{
    Task ExecuteAsync(
        ScheduleTask scheduleTask,
        bool forceRun = false,
        bool throwException = false,
        bool ensureRunOncePerPeriod = true);
}
