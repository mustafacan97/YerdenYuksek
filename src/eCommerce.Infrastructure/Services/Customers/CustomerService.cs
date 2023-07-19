using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives;
using Microsoft.EntityFrameworkCore;
using eCommerce.Core.Entities.Security;
using eCommerce.Core.Entities.Configuration.CustomSettings;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Shared;
using eCommerce.Core.Services.Caching;
using eCommerce.Core.Services.Security;
using eCommerce.Infrastructure.Services.Secuirty;
using eCommerce.Core.Services.Customers;

namespace eCommerce.Infrastructure.Services.Customers;

public class CustomerService : ICustomerService
{
    #region Fields

    private readonly CustomerSettings _customerSettings;

    private readonly IEncryptionService _encryptionService;

    private readonly IStaticCacheManager _staticCacheManager;

    private readonly IWebHelper _webHelper;

    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Constructure and Destructure

    public CustomerService(
        CustomerSettings customerSettings,
        IEncryptionService encryptionService,
        IStaticCacheManager staticCacheManager,
        IWebHelper webHelper,
        IUnitOfWork unitOfWork)
    {
        _customerSettings = customerSettings;
        _encryptionService = encryptionService;
        _staticCacheManager = staticCacheManager;
        _webHelper = webHelper;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Public Methods

    public async Task<Result<Customer>> RegisterCustomerAsync(string email, string password)
    {
        if (await GetCustomerByEmailAsync(email) is not null)
        {
            return Result.Failure(Error.Conflict(description: "Email is already registered!"));
        }

        var saltKey = EncryptionService.CreateSaltKey(YerdenYuksekCustomerServicesDefaults.PasswordSaltKeySize);
        var customer = Customer.Create(email);
        var customerPassword = new CustomerSecurity
        {
            CustomerId = customer.Id,
            PasswordSalt = saltKey,
            Password = EncryptionService.CreatePasswordHash(password, saltKey, _customerSettings.HashedPasswordFormat),
            LastIpAddress = _webHelper.GetCurrentIpAddress(),
        };

        var registeredRole = await GetCustomerRoleByNameAsync(RoleDefaults.RegisteredRoleName);
        if (registeredRole is null)
        {
            return Result.Failure(Error.Failure(description: "Related customer role not found!"));
        }

        customer.SetCustomerSecurity(customerPassword);
        customer.SetCustomerRole(registeredRole);

        await InsertCustomerAsync(customer);

        return Result.Success(customer);
    }

    public async Task InsertCustomerAsync(Customer customer)
    {
        await _unitOfWork.GetRepository<Customer>().InsertAsync(customer);
        await _unitOfWork.SaveChangesAsync();
    }

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

            _unitOfWork.GetRepository<Customer>().Update(customer);
            await _unitOfWork.SaveChangesAsync();

            return Result.Failure(Error.Failure(description: "Email or password is not correect!"));
        }

        customer.CustomerSecurity.FailedLoginAttempts = 0;
        customer.CustomerSecurity.CannotLoginUntilDateUtc = null;
        customer.CustomerSecurity.RequireReLogin = false;
        customer.CustomerSecurity.LastIpAddress = _webHelper.GetCurrentIpAddress();
        customer.LastLoginDateUtc = DateTime.UtcNow;

        _unitOfWork.GetRepository<Customer>().Update(customer);

        return Result.Success();
    }

    public async Task<Customer?> GetCustomerByEmailAsync(string email, bool includeDeleted = false)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<Customer>.ByEmailCacheKey, email);

        async Task<Customer?> getEntityAsync()
        {
            var query = _unitOfWork.GetRepository<Customer>();

            if (includeDeleted)
            {
                return await query.Table.FirstOrDefaultAsync(q => q.Email == email);
            }

            if (typeof(Customer).GetInterface(nameof(ISoftDeletedEntity)) == null)
            {
                return await query.Table.FirstOrDefaultAsync(q => q.Email == email);
            }

            return await query.Table.FirstOrDefaultAsync(q => q.Email == email && !q.Deleted);
        }

        return await _staticCacheManager.GetAsync(cacheKey, getEntityAsync);
    }

    public async Task<Role?> GetCustomerRoleByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var key = _staticCacheManager.PrepareKeyForDefaultCache(YerdenYuksekCustomerServicesDefaults.CustomerRolesByNameCacheKey, name);

        var query = from cr in _unitOfWork.GetRepository<Role>().Table
                    orderby cr.Id
                    where cr.Name == name
                    select cr;

        var customerRole = await _staticCacheManager.GetAsync(key, async () => await query.FirstOrDefaultAsync());

        return customerRole;
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

        var savedPassword = EncryptionService.CreatePasswordHash(enteredPassword,
                                                                 customerSecurity.PasswordSalt,
                                                                 _customerSettings.HashedPasswordFormat);

        return customerSecurity.Password.Equals(savedPassword);
    }

    #endregion
}
