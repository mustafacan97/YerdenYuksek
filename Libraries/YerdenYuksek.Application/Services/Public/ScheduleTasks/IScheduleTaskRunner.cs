using YerdenYuksek.Core.Domain.ScheduleTasks;

namespace YerdenYuksek.Application.Services.Public.ScheduleTasks;

public interface IScheduleTaskRunner
{
    Task ExecuteAsync(
        ScheduleTask scheduleTask,
        bool forceRun = false,
        bool throwException = false,
        bool ensureRunOncePerPeriod = true);
}
