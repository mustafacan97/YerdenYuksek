using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Common;

public class Address : BaseEntity
{
    #region Public Properties

    public Guid CustomerId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public Guid CountryId { get; set; }

    public Guid City { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public string ZipCode { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    #endregion
}
