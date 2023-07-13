using eCommerce.Application.Services.ScheduleTasks;
using eCommerce.Core.Domain.ScheduleTasks;
using eCommerce.Core.Interfaces;
using YerdenYuksek.Core.Caching;
using YerdenYuksek.Web.Framework.Infrastructure;

namespace eCommerce.Infrastructure.Persistence.Services.ScheduleTasks;

public class ScheduleTaskRunner : IScheduleTaskRunner
{
    #region Fields

    private readonly ILocker _locker;

    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Constructure and Destructure

    public ScheduleTaskRunner(
        ILocker locker,
        IUnitOfWork unitOfWork)
    {
        _locker = locker;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Public Methods

    public async Task ExecuteAsync(
        ScheduleTask scheduleTask,
        bool forceRun = false,
        bool throwException = false,
        bool ensureRunOncePerPeriod = true)
    {
        if (scheduleTask is null) return;

        var enabled = forceRun || (scheduleTask.Active && !scheduleTask.Deleted);

        if (!enabled) return;

        if (ensureRunOncePerPeriod)
        {
            if (IsTaskAlreadyRunning(scheduleTask)) return;
            if (scheduleTask.LastStartUtc.HasValue && (DateTime.UtcNow - scheduleTask.LastStartUtc).Value.TotalSeconds < scheduleTask.Seconds) return;
        }

        try
        {
            var expirationInSeconds = Math.Min(scheduleTask.Seconds, 300) - 1;
            var expiration = TimeSpan.FromSeconds(expirationInSeconds);

            await _locker.PerformActionWithLockAsync(scheduleTask.Type, expiration, () => PerformTaskAsync(scheduleTask));
        }
        catch (Exception exc)
        {
            throw new Exception(exc.Message, exc);
        }
    }

    #endregion

    #region Methods

    private async Task PerformTaskAsync(ScheduleTask scheduleTask)
    {
        var type = Type.GetType(scheduleTask.Type) ??
                   AppDomain.CurrentDomain.GetAssemblies()
                       .Select(a => a.GetType(scheduleTask.Type))
                       .FirstOrDefault(t => t != null);

        if (type is null)
        {
            throw new Exception($"Schedule task ({scheduleTask.Type}) cannot by instantiated");
        }

        object? instance = null;

        try
        {
            instance = EngineContext.Current.Resolve(type);
        }
        catch
        {
            // ignored
        }

        instance ??= EngineContext.Current.ResolveUnregistered(type);

        if (instance is not IScheduleTask task)
        {
            return;
        }

        scheduleTask.LastStartUtc = DateTime.UtcNow;
        _unitOfWork.GetRepository<ScheduleTask>().Update(scheduleTask);

        await task.ExecuteAsync();

        scheduleTask.LastEndUtc = scheduleTask.LastSuccessUtc = DateTime.UtcNow;
        _unitOfWork.GetRepository<ScheduleTask>().Update(scheduleTask);

        await _unitOfWork.SaveChangesAsync();
    }

    private static bool IsTaskAlreadyRunning(ScheduleTask scheduleTask)
    {
        //task run for the first time
        if (!scheduleTask.LastStartUtc.HasValue && !scheduleTask.LastEndUtc.HasValue)
        {
            return false;
        }

        var lastStartUtc = scheduleTask.LastStartUtc ?? DateTime.UtcNow;

        //task already finished
        if (scheduleTask.LastEndUtc.HasValue && lastStartUtc < scheduleTask.LastEndUtc)
        {
            return false;
        }

        //task wasn't finished last time
        if (lastStartUtc.AddSeconds(scheduleTask.Seconds) <= DateTime.UtcNow)
        {
            return false;
        }

        return true;
    }

    #endregion
}
