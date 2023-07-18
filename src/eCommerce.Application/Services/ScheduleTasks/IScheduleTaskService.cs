using eCommerce.Core.Entities.ScheduleTasks;

namespace eCommerce.Application.Services.ScheduleTasks;

public interface IScheduleTaskService
{
    #region Commands

    Task DeleteTaskAsync(ScheduleTask task);

    Task InsertTaskAsync(ScheduleTask task);

    Task UpdateTaskAsync(ScheduleTask task);

    #endregion

    #region Queries

    Task<ScheduleTask> GetTaskByIdAsync(Guid taskId);

    Task<ScheduleTask> GetTaskByTypeAsync(string type);

    Task<IList<ScheduleTask>> GetAllTasksAsync(bool onlyActive = true, bool includeDeleted = false);

    #endregion
}