using eCommerce.Core.Entities.Orders;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Common;

public class Address : SoftDeletedEntity
{
    #region Constructure and Destructure

    public Address()
    {
        BillingOrders = new HashSet<Order>();
        ShippingOrders = new HashSet<Order>();
    }

    #endregion

    #region Public Properties

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public Guid CustomerId { get; set; }

    public Guid CountryId { get; set; }

    public Guid CityId { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public string ZipCode { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public Country Country { get; set; }

    public City City { get; set; }

    public ICollection<Order> BillingOrders { get; set; }

    public ICollection<Order> ShippingOrders { get; set; }

    #endregion
}
