using eCommerce.Core.Configuration;

namespace eCommerce.Core.Domain.Configuration.CustomSettings;

public class EmailAccountSettings : ISettings
{
    public Guid DefaultEmailAccountId { get; set; }
}
