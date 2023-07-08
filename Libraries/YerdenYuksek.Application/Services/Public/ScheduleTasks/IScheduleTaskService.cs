using YerdenYuksek.Core.Domain.ScheduleTasks;

namespace YerdenYuksek.Application.Services.Public.ScheduleTasks;

public interface IScheduleTaskService
{
    Task DeleteTaskAsync(ScheduleTask task);

    Task<ScheduleTask> GetTaskByIdAsync(Guid taskId);

    Task<ScheduleTask> GetTaskByTypeAsync(string type);

    Task<IList<ScheduleTask>> GetAllTasksAsync(bool showHidden = false);

    Task InsertTaskAsync(ScheduleTask task);

    Task UpdateTaskAsync(ScheduleTask task);
}