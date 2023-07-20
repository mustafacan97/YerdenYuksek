using eCommerce.Core.Entities.ScheduleTasks;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Services.ScheduleTasks;
using Microsoft.EntityFrameworkCore;
namespace eCommerce.Infrastructure.Services.ScheduleTasks;

public class ScheduleTaskService : IScheduleTaskService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Constructure and Destructure

    public ScheduleTaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Public Methods

    public async Task DeleteTaskAsync(ScheduleTask task)
    {
        _unitOfWork.GetRepository<ScheduleTask>().Delete(task);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ScheduleTask> GetTaskByIdAsync(Guid taskId)
    {
        return await _unitOfWork.GetRepository<ScheduleTask>().GetByIdAsync(taskId);
    }

    public async Task<ScheduleTask> GetTaskByTypeAsync(string type)
    {
        var query = from st in _unitOfWork.GetRepository<ScheduleTask>().Table
                    where st.Type == type
                    select st;

        return await query.FirstAsync();
    }

    public async Task<IList<ScheduleTask>> GetAllTasksAsync(bool onlyActive = true, bool includeDeleted = false)
    {
        var tasks = await _unitOfWork.GetRepository<ScheduleTask>().GetAllAsync(query =>
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

    public async Task InsertTaskAsync(ScheduleTask task)
    {
        if (task is null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        await _unitOfWork.GetRepository<ScheduleTask>().InsertAsync(task);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateTaskAsync(ScheduleTask task)
    {
        _unitOfWork.GetRepository<ScheduleTask>().Update(task);
        await _unitOfWork.SaveChangesAsync();
    }

    #endregion
}