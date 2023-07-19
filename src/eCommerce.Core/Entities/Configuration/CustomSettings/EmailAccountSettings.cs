using eCommerce.Core.Interfaces;

namespace eCommerce.Core.Entities.Configuration.CustomSettings;

public class EmailAccountSettings : ISettings
{
    public Guid DefaultEmailAccountId { get; set; }
}
