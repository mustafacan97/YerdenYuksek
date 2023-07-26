using eCommerce.Core.DomainEvents;
using eCommerce.Core.Entities.Common;
using eCommerce.Core.Entities.Directory;
using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Entities.Logging;
using eCommerce.Core.Entities.Media;
using eCommerce.Core.Entities.Orders;
using eCommerce.Core.Entities.Security;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Customers;

public class Customer : SoftDeletedEntity
{
    #region Constructure and Destructure

    public Customer()
    {
        Logs = new HashSet<Log>();
        ActivityLogs = new HashSet<ActivityLog>();
        Addresses = new HashSet<Address>();
        CustomerRoles = new HashSet<Role>();
        Orders = new HashSet<Order>();
        ShoppingCartItems = new HashSet<ShoppingCartItem>();
    }

    #endregion

    #region Public Properties

    public string Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? PhoneNumber { get; set; }

    public bool EmailValidated { get; set; }

    public bool PhoneNumberValidated { get; set; }

    public Guid? LanguageId { get; set; }

    public Guid? CurrencyId { get; set; }

    public Guid? DefaultAddressId { get; set; }

    public Guid? PictureId { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? LastLoginDateUtc { get; set; }

    public DateTime? LastActivityDateUtc { get; set; }

    public Language? Language { get; set; }

    public Currency? Currency { get; set; }

    public Address? DefaultAddress { get; set; }

    public CustomerSecurity CustomerSecurity { get; private set; }

    public Picture Picture { get; set; }

    public ICollection<Log> Logs { get; set; }

    public ICollection<ActivityLog> ActivityLogs { get; set; }

    public ICollection<Address> Addresses { get; set; }

    public ICollection<Role> CustomerRoles { get; private set; }

    public ICollection<Order> Orders { get; set; }

    public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

    #endregion

    #region Public Methods

    public static Customer Create(string email, CustomerSecurity security, Role role)
    {
        var newCustomer = Create(email);
        newCustomer.SetCustomerSecurity(newCustomer.Id, security);
        newCustomer.SetCustomerRole(role);
        newCustomer.RaiseDomainEvent(new CustomerCreatedDomainEvent(newCustomer.Id));

        return newCustomer;
    }

    public Customer SetCustomerSecurity(Guid customerId, CustomerSecurity customerSecuirty)
    {
        customerSecuirty.SetCustomerId(customerId);
        CustomerSecurity = customerSecuirty;
        return this;
    }

    public Customer SetCustomerRole(Role role)
    {
        if (CustomerRoles.FirstOrDefault(q => q.Id == role.Id) is null)
        {
            CustomerRoles.Add(role);
        }
        return this;
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

    #region Methods

    private static Customer Create(string email)
    {
        return new Customer()
        {
            Email = email,
            CreatedOnUtc = DateTime.UtcNow,
            Active = true,
            Deleted = false            
        };
    }

    #endregion
}