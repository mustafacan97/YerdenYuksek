using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Common;

public class Address : SoftDeletedEntity
{
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

    #endregion
}
