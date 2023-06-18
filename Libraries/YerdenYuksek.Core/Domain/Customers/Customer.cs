using YerdenYuksek.Core.Domain.Stores;
using YerdenYuksek.Core.Domain.Common;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Customers;

public class Customer : BaseEntity, ISoftDeletedEntity
{
    #region Constructure and Destructure

    public Customer()
    {
        AllAddresses = new HashSet<Address>();
    }

    #endregion

    #region Public Properties

    public string Username { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public string Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public bool EmailValidated { get; set; }

    public bool PhoneNumberValidated { get; set; }

    public bool RequireReLogin { get; set; }

    public int FailedLoginAttempts { get; set; }

    public DateTime? CannotLoginUntilDateUtc { get; set; }

    public bool Active { get; set; }

    public bool Deleted { get; set; }

    public bool IsSystemAccount { get; set; }

    public string SystemName { get; set; }

    public string LastIpAddress { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? LastLoginDateUtc { get; set; }

    public DateTime LastActivityDateUtc { get; set; }

    public Guid RegisteredInStoreId { get; set; }

    public Guid? SelectedAddressId { get; set; }

    public Guid? BillingAddressId { get; set; }

    public Guid? ShippingAddressId { get; set; }

    public CustomerPassword CustomerPassword { get; set; }

    public ICollection<Address> AllAddresses { get; set; }

    #endregion
}