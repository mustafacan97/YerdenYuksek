namespace eCommerce.Core.Caching;

public interface ILocker
{
    Task<bool> PerformActionWithLockAsync(string resource, TimeSpan expirationTime, Func<Task> action);

    Task RunWithHeartbeatAsync(
        string key,
        TimeSpan expirationTime,
        TimeSpan heartbeatInterval,
        Func<CancellationToken, Task> action,
        CancellationTokenSource? cancellationTokenSource = default);

    Task CancelTaskAsync(string key, TimeSpan expirationTime);

    Task<bool> IsTaskRunningAsync(string key);
}
