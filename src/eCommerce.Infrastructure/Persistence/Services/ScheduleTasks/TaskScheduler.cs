using eCommerce.Application.Services.ScheduleTasks;
using eCommerce.Core.Entities.ScheduleTasks;
using eCommerce.Core.Interfaces;

namespace eCommerce.Infrastructure.Persistence.Services.ScheduleTasks;

public class TaskScheduler : ITaskScheduler
{
    #region Fields

    private static readonly List<TaskThread> _taskThreads = new();

    private readonly IUnitOfWork _unitOfWork;

    private readonly IScheduleTaskRunner _scheduleTaskRunner;

    #endregion

    #region Constructure and Destructure

    public TaskScheduler(
        IUnitOfWork unitOfWork, 
        IScheduleTaskRunner scheduleTaskRunner)
    {
        _unitOfWork = unitOfWork;
        _scheduleTaskRunner = scheduleTaskRunner;
    }

    #endregion

    #region Public Methods

    public async Task InitializeAsync()
    {
        if (_taskThreads.Any())
        {
            return;
        }

        var scheduleTasks = (await _unitOfWork.GetRepository<ScheduleTask>().GetAllAsync())
            .OrderBy(x => x.Seconds)
            .ToList();

        foreach (var scheduleTask in scheduleTasks)
        {
            var taskThread = new TaskThread(scheduleTask, _scheduleTaskRunner)
            {
                Seconds = scheduleTask.Seconds
            };

            if (scheduleTask.LastStartUtc.HasValue)
            {
                var secondsLeft = (DateTime.UtcNow - scheduleTask.LastStartUtc).Value.TotalSeconds;

                if (secondsLeft >= scheduleTask.Seconds)
                {
                    taskThread.InitSeconds = 0;
                }
                else
                {
                    taskThread.InitSeconds = (int)(scheduleTask.Seconds - secondsLeft) + 1;
                }
            }
            else
            {
                taskThread.InitSeconds = scheduleTask.Seconds;
            }

            _taskThreads.Add(taskThread);
        }
    }

    public void StartScheduler()
    {
        foreach (var taskThread in _taskThreads)
        {
            taskThread.InitTimer();
        }
    }

    public void StopScheduler()
    {
        foreach (var taskThread in _taskThreads)
        {
            taskThread.Dispose();
        }
    }

    #endregion

    #region Nested class

    protected class TaskThread : IDisposable
    {
        #region Fields

        private readonly IScheduleTaskRunner _scheduleTaskRunner;

        private readonly ScheduleTask _scheduleTask;

        private Timer _timer;

        private bool _disposed;

        #endregion

        #region Constructure and Destructure

        public TaskThread(
            ScheduleTask task,
            IScheduleTaskRunner scheduleTaskRunner)
        {
            Seconds = 10 * 60;
            _scheduleTask = task;
            _scheduleTaskRunner = scheduleTaskRunner;
        }

        #endregion

        #region Public Properties

        public int Seconds { get; set; }

        public int InitSeconds { get; set; }

        public DateTime StartedUtc { get; protected set; }

        public bool IsRunning { get; protected set; }

        public int Interval
        {
            get
            {
                //if somebody entered more than "2147483" seconds, then an exception could be thrown (exceeds int.MaxValue)
                var interval = Seconds * 1000;
                if (interval <= 0)
                {
                    interval = int.MaxValue;
                }

                return interval;
            }
        }

        public int InitInterval
        {
            get
            {
                //if somebody entered less than "0" seconds, then an exception could be thrown
                var interval = InitSeconds * 1000;
                if (interval <= 0)
                {
                    interval = 0;
                }

                return interval;
            }
        }

        public bool RunOnlyOnce { get; set; }

        public bool IsStarted => _timer != null;

        public bool IsDisposed => _disposed;

        #endregion        

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void InitTimer()
        {
            _timer ??= new Timer(TimerHandler, null, InitInterval, Interval);
        }

        #endregion

        #region Methods

        private async Task RunAsync()
        {
            if (Seconds <= 0)
            {
                return;
            }

            StartedUtc = DateTime.UtcNow;
            IsRunning = true;

            try
            {
                await _scheduleTaskRunner.ExecuteAsync(_scheduleTask);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            IsRunning = false;
        }

        private void TimerHandler(object state)
        {
            try
            {
                _timer.Change(-1, -1);

                RunAsync().Wait();
            }
            catch
            {
                // ignore
            }
            finally
            {
                if (!_disposed && _timer != null)
                {
                    if (RunOnlyOnce)
                        Dispose();
                    else
                        _timer.Change(Interval, Interval);
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                lock (this)
                {
                    _timer?.Dispose();
                }
            }

            _disposed = true;
        }

        #endregion
    }

    #endregion
}
