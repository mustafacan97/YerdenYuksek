using eCommerce.Core.Domain.Directory;
using eCommerce.Core.Domain.Logging;
using eCommerce.Core.Domain.Security;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives;
using YerdenYuksek.Core.Domain.Common;
using YerdenYuksek.Core.Domain.Logging;

namespace eCommerce.Core.Domain.Customers;

public class Customer : SoftDeletedEntity, ISoftDeletedEntity
{
    #region Constructure and Destructure

    public Customer()
    {
        Addresses = new HashSet<Address>();
        CustomerRoles = new HashSet<Role>();
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

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? LastLoginDateUtc { get; set; }

    public DateTime? LastActivityDateUtc { get; set; }

    public Guid? CurrencyId { get; set; }

    public Currency? Currency { get; set; }

    public CustomerSecurity CustomerSecurity { get; private set; }

    public ICollection<Address> Addresses { get; set; }

    public ICollection<Log> Logs { get; set; }

    public ICollection<ActivityLog> ActivityLogs { get; set; }

    public ICollection<Role> CustomerRoles { get; private set; }

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
            CustomerSecurity.LastIpAddress = ipAddress;
        }
    }

    public void SetCustomerPassword(CustomerSecurity customerSecuirty)
    {
        CustomerSecurity = customerSecuirty;
    }

    public void SetCustomerRole(Role customerRole)
    {
        var isAlreadyExists = CustomerRoles.FirstOrDefault(q => q.Id == customerRole.Id);
        if (isAlreadyExists is null)
        {
            CustomerRoles.Add(customerRole);
        }
    }

    public bool CustomerHasSpecifiedRole(string roleName)
    {
        return CustomerRoles.FirstOrDefault(cr => cr.Name == roleName) is not null;
    }

    public bool CustomerHasLoginRestrictions()
    {
        return CustomerSecurity.CannotLoginUntilDateUtc.HasValue &&
            CustomerSecurity.CannotLoginUntilDateUtc.Value > DateTime.UtcNow;
    }

    public void LockOutCustomerAccount(double lockOutMinutes)
    {
        CustomerSecurity.CannotLoginUntilDateUtc = DateTime.UtcNow.AddMinutes(lockOutMinutes);
        CustomerSecurity.FailedLoginAttempts = 0;
    }

    #endregion
}