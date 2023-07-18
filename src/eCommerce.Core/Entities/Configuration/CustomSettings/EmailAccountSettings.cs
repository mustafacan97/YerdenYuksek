using eCommerce.Core.Configuration;

namespace eCommerce.Core.Entities.Configuration.CustomSettings;

public class EmailAccountSettings : ISettings
{
    public Guid DefaultEmailAccountId { get; set; }
}
