namespace YerdenYuksek.Application.Services.Public.ScheduleTasks;

public interface ITaskScheduler
{
    Task InitializeAsync();

    public void StartScheduler();

    public void StopScheduler();
}
