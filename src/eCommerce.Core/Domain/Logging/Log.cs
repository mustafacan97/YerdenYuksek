using eCommerce.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Logging;

public class Log : BaseEntity
{
    #region Public Properties

    public Guid CustomerId { get; set; }

    public string ShortMessage { get; set; }

    public string? FullMessage { get; set; }

    public string? IpAddress { get; set; }

    public int LogLevelId { get; set; }

    public string EndpointUrl { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public LogLevel LogLevel
    {
        get => (LogLevel)LogLevelId;
        set => LogLevelId = (int)value;
    }

    #endregion
}
