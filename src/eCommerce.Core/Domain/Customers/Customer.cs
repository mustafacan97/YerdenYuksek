using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives;
using YerdenYuksek.Core.Domain.Common;
using YerdenYuksek.Core.Domain.Logging;

namespace YerdenYuksek.Core.Domain.Customers;

public class Customer : SoftDeletedEntity, ISoftDeletedEntity
{
    #region Constructure and Destructure

    public Customer()
    {
        Addresses = new HashSet<Address>();
        CustomerRoles = new HashSet<CustomerRole>();
        Logs = new HashSet<Log>();
        ActivityLogs = new HashSet<ActivityLog>();
    }

    #endregion

    #region Public Properties

    public string Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public bool EmailValidated { get; set; }

    public bool PhoneNumberValidated { get; set; }

    public bool RequireReLogin { get; set; }

    public int FailedLoginAttempts { get; set; }

    public DateTime? CannotLoginUntilDateUtc { get; set; }

    public bool Active { get; set; }

    public bool Deleted { get; set; }

    public string? LastIpAddress { get; private set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? LastLoginDateUtc { get; set; }

    public DateTime? LastActivityDateUtc { get; set; }

    public CustomerPassword CustomerPassword { get; private set; }

    public ICollection<Address> Addresses { get; set; }

    public ICollection<Log> Logs { get; set; }

    public ICollection<ActivityLog> ActivityLogs { get; set; }

    public ICollection<CustomerRole> CustomerRoles { get; private set; }

    #endregion

    #region Public Methods

    public static Customer Create(string email)
    {
        return new Customer()
        {
            Email = email,
            Active = true,
            CreatedOnUtc = DateTime.UtcNow
        };
    }

    public void SetIpAddress(string ipAddress)
    {
        if (!string.IsNullOrEmpty(ipAddress))
        {
            LastIpAddress = ipAddress;
        }
    }

    public void SetCustomerPassword(CustomerPassword customerPassword)
    {
        CustomerPassword = customerPassword;
    }

    public void SetCustomerRole(CustomerRole customerRole)
    {
        var isAlreadyExists = CustomerRoles.FirstOrDefault(q => q.Id == customerRole.Id);
        if (isAlreadyExists is null)
        {
            CustomerRoles.Add(customerRole);
        }
    }

    #endregion
}