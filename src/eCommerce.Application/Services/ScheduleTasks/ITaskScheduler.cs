namespace eCommerce.Application.Services.ScheduleTasks;

public interface ITaskScheduler
{
    Task InitializeAsync();

    public void StartScheduler();

    public void StopScheduler();
}
