using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Logging;

public class ActivityLogType : BaseEntity
{
    #region Constructure and Destructure

    public ActivityLogType()
    {
        ActivityLogs = new HashSet<ActivityLog>();
    }

    #endregion

    #region Public Properties

    public string SystemKeyword { get; set; }

    public string Name { get; set; }

    public ICollection<ActivityLog> ActivityLogs { get; set; }

    #endregion
}
