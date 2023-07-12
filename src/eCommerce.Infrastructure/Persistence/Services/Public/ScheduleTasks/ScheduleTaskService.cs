using Microsoft.EntityFrameworkCore;
using YerdenYuksek.Application.Services.Public.ScheduleTasks;
using YerdenYuksek.Core.Domain.ScheduleTasks;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Web.Framework.Persistence.Services.Public.ScheduleTasks;

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
        return await _unitOfWork.GetRepository<ScheduleTask>().GetByIdAsync(taskId, _ => default);
    }

    public async Task<ScheduleTask> GetTaskByTypeAsync(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            return null;
        }

        var query = from st in _unitOfWork.GetRepository<ScheduleTask>().Table
                    where st.Type == type
                    orderby st.Id descending
                    select st;

        return await query.FirstOrDefaultAsync();
    }

    public async Task<IList<ScheduleTask>> GetAllTasksAsync(bool showHidden = false)
    {
        var tasks = await _unitOfWork.GetRepository<ScheduleTask>().GetAllAsync(query =>
        {
            if (!showHidden)
            {
                query = query.Where(task => task.Enabled);
            }

            query = query.OrderByDescending(t => t.Seconds);

            return query;
        });

        return tasks;
    }

    public async Task InsertTaskAsync(ScheduleTask task)
    {
        if (task is null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        if (task.Enabled && !task.LastEnabledUtc.HasValue)
        {
            task.LastEnabledUtc = DateTime.UtcNow;
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