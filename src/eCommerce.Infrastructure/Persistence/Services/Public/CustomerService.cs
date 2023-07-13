using eCommerce.Core.Caching;
using eCommerce.Core.Domain.Configuration.CustomSettings;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives;
using Microsoft.EntityFrameworkCore;
using YerdenYuksek.Application.Models.Customers;
using YerdenYuksek.Application.Services.Public.Customers;
using YerdenYuksek.Application.Services.Public.Security;
using eCommerce.Core.Caching;
using YerdenYuksek.Core.Domain.Customers;

namespace YerdenYuksek.Web.Framework.Persistence.Services.Public;

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

    #region Commands

    public async Task<RegisterResponseModel> RegisterCustomerAsync(string email, string password)
    {
        var result = new RegisterResponseModel();

        if (await GetCustomerByEmailAsync(email) is not null)
        {
            result.AddError(Error.Conflict(description: "Email is already registered!"));
            return result;
        }

        var saltKey = _encryptionService.CreateSaltKey(YerdenYuksekCustomerServicesDefaults.PasswordSaltKeySize);
        var customer = Customer.Create(email);
        var customerPassword = new CustomerSecurity
        {
            CustomerId = customer.Id,
            PasswordSalt = saltKey,
            Password = _encryptionService.CreatePasswordHash(password, saltKey, _customerSettings.HashedPasswordFormat),
        };

        var registeredRole = await GetCustomerRoleByNameAsync(CustomerDefaults.RegisteredRoleName);
        if (registeredRole is null)
        {
            result.AddError(Error.Failure(description: "Related customer role not found!"));
            return result;
        }

        customer.SetIpAddress(_webHelper.GetCurrentIpAddress());
        customer.SetCustomerPassword(customerPassword);
        customer.SetCustomerRole(registeredRole);

        await InsertCustomerAsync(customer);

        result.SetCustomer(customer);

        return result;
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
        if (customer.CustomerHasSpecifiedRole(CustomerDefaults.RegisteredRoleName))
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

    #endregion

    #region Queries

    public async Task<Customer?> GetCustomerByEmailAsync(string email, bool includeDeleted = false)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(CustomerEntityCacheDefaults.ByEmailCacheKey, email);

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

    public async Task<CustomerRole?> GetCustomerRoleByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var key = _staticCacheManager.PrepareKeyForDefaultCache(YerdenYuksekCustomerServicesDefaults.CustomerRolesByNameCacheKey, name);

        var query = from cr in _unitOfWork.GetRepository<CustomerRole>().Table
                    orderby cr.Id
                    where cr.Name == name
        select cr;

        var customerRole = await _staticCacheManager.GetAsync(key, async () => await query.FirstOrDefaultAsync());

        return customerRole;
    }

    #endregion

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

        var savedPassword = _encryptionService.CreatePasswordHash(enteredPassword,
                                                                  customerSecurity.PasswordSalt,
                                                                  _customerSettings.HashedPasswordFormat);

        return customerSecurity.Password.Equals(savedPassword);
    }

    #endregion
}
