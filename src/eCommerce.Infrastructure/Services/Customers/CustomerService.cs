using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives;
using eCommerce.Core.Entities.Security;
using eCommerce.Core.Entities.Configuration.CustomSettings;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Shared;
using eCommerce.Core.Services.Caching;
using eCommerce.Core.Services.Customers;

namespace eCommerce.Infrastructure.Services.Customers;

public class CustomerService : ICustomerService
{
    #region Fields

    private readonly CustomerSettings _customerSettings;

    private readonly IStaticCacheManager _staticCacheManager;

    private readonly IWebHelper _webHelper;

    private readonly IRepository<Customer> _customerRepository;

    #endregion

    #region Constructure and Destructure

    public CustomerService(
        CustomerSettings customerSettings,
        IStaticCacheManager staticCacheManager,
        IWebHelper webHelper,
        IRepository<Customer> customerRepository)
    {
        _customerSettings = customerSettings;
        _staticCacheManager = staticCacheManager;
        _webHelper = webHelper;
        _customerRepository = customerRepository;
    }

    #endregion

    #region Public Methods

    public async Task<Result> ValidateCustomerAsync(string email, string password)
    {
        var customer = await GetCustomerByEmailAsync(email);
        if (customer is null)
        {
            return Result.NotFound(Error.NotFound(description: "Customer not found!"));
        }

        if (customer.Deleted)
        {
            return Result.NotFound(Error.Failure(description: "Customer is deleted!"));
        }

        if (!customer.Active)
        {
            return Result.NotFound(Error.Failure(description: "Customer is active!"));
        }

        // Only registered can login
        if (customer.CustomerHasSpecifiedRole(RoleDefaults.RegisteredRoleName))
        {
            return Result.Forbidden(Error.Failure(description: "Customer must be registered!"));
        }

        var maxFailedLoginAttempts = _customerSettings.FailedPasswordAllowedAttempts;

        //check whether a customer is locked out
        if (customer.CustomerHasLoginRestrictions())
        {
            return Result.Forbidden(Error.Failure(description: "Account is locked out!"));
        }

        if (!PasswordsMatch(customer.CustomerSecurity, password))
        {
            customer.CustomerSecurity.FailedLoginAttempts++;
            if (_customerSettings.FailedPasswordAllowedAttempts > 0 &&
                customer.CustomerSecurity.FailedLoginAttempts >= maxFailedLoginAttempts)
            {
                customer.LockOutCustomerAccount(_customerSettings.FailedPasswordLockoutMinutes);
            }

            await _customerRepository.UpdateAsync(customer);

            return Result.Failure(Error.Failure(description: "Email or password is not correect!"));
        }

        customer.CustomerSecurity.FailedLoginAttempts = 0;
        customer.CustomerSecurity.CannotLoginUntilDateUtc = null;
        customer.CustomerSecurity.RequireReLogin = false;
        customer.CustomerSecurity.LastIpAddress = _webHelper.GetCurrentIpAddress();
        customer.LastLoginDateUtc = DateTime.UtcNow;

        await _customerRepository.UpdateAsync(customer);

        return Result.Success();
    }

    public async Task<Customer?> GetCustomerByEmailAsync(string email, bool includeDeleted = false)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<Customer>.ByEmailCacheKey, email);

        return await _customerRepository.GetFirstOrDefaultAsync(
            func: q => q.Where(p => p.Email == email),
            getCacheKey: q => q.PrepareKey(EntityCacheDefaults<Customer>.ByEmailCacheKey, email));
    }

    #endregion

    #region Methods

    private bool PasswordsMatch(CustomerSecurity customerSecurity, string enteredPassword)
    {
        if (customerSecurity is null || string.IsNullOrEmpty(enteredPassword))
        {
            return false;
        }

        if (customerSecurity.Password is null)
        {
            return false;
        }

        var savedPassword = EncryptionHelper.CreatePasswordHash(enteredPassword,
                                                                 customerSecurity.PasswordSalt,
                                                                 _customerSettings.HashedPasswordFormat);

        return customerSecurity.Password.Equals(savedPassword);
    }

    #endregion
}
