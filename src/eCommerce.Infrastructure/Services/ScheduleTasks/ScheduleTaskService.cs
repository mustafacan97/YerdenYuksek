using eCommerce.Core.Entities.ScheduleTasks;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Services.ScheduleTasks;
namespace eCommerce.Infrastructure.Services.ScheduleTasks;

public class ScheduleTaskService : IScheduleTaskService
{
    #region Fields

    private readonly IRepository<ScheduleTask> _scheduleTaskService;

    #endregion

    #region Constructure and Destructure

    public ScheduleTaskService(IRepository<ScheduleTask> scheduleTaskService)
    {
        _scheduleTaskService = scheduleTaskService;
    }

    #endregion

    #region Public Methods

    public async Task DeleteTaskAsync(ScheduleTask task) => await _scheduleTaskService.DeleteAsync(task);

    public async Task<ScheduleTask> GetTaskByIdAsync(Guid taskId) => await _scheduleTaskService.GetByIdAsync(taskId);

    public async Task<ScheduleTask> GetTaskByTypeAsync(string type)
    {
        return await _scheduleTaskService.GetFirstOrDefaultAsync(q => q.Where(p => p.Type == type));
    }

    public async Task<IList<ScheduleTask>> GetAllTasksAsync(bool onlyActive = true, bool includeDeleted = false)
    {
        var tasks = await _scheduleTaskService.GetAllAsync(query =>
        {
            if (onlyActive)
            {
                query = query.Where(t => t.Active && !t.Deleted);
            }
            else if (!includeDeleted)
            {
                query = query.Where(t => !t.Deleted);
            }

            return query.OrderByDescending(t => t.Seconds);
        });

        return tasks;
    }

    public async Task InsertTaskAsync(ScheduleTask task) => await _scheduleTaskService.InsertAsync(task);

    public async Task UpdateTaskAsync(ScheduleTask task) => await _scheduleTaskService.UpdateAsync(task);

    #endregion
}