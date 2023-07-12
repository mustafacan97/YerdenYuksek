using eCommerce.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Logging;

public class ActivityLogType : BaseEntity
{
    #region Public Properties

    public string SystemKeyword { get; set; }

    public string Name { get; set; }

    public bool Active { get; set; }

    #endregion
}
