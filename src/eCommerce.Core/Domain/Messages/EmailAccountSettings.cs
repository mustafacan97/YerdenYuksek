using YerdenYuksek.Core.Configuration;

namespace YerdenYuksek.Core.Domain.Messages;

public class EmailAccountSettings : ISettings
{
    public static Guid DefaultEmailAccountId { get; set; } = Guid.Parse("fd5134ba-eb25-4c0d-81e1-b9639c1267c8");
}
