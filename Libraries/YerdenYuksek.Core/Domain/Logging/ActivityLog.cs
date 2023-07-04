using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Logging;

public class ActivityLog : BaseEntity
{
    #region Public Properties

    public Guid ActivityLogTypeId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid? EntityId { get; set; }

    public string IpAddress { get; set; }

    public string Comment { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public ActivityLogType ActivityLogType { get; set; }

    #endregion
}