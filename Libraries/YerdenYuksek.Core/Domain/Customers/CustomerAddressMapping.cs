using YerdenYuksek.Core.Domain.Common;

namespace YerdenYuksek.Core.Domain.Customers;

public class CustomerAddressMapping
{
    #region Public Properties

    public Guid CustomerId { get; set; }

    public Guid AddressId { get; set; }

    public Address Address { get; set; }

    #endregion
}
