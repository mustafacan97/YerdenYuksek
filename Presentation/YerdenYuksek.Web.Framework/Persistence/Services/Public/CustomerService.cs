using Microsoft.EntityFrameworkCore;
using YerdenYuksek.Application.Services.Public.Customers;
using YerdenYuksek.Application.Services.Public.Security;
using YerdenYuksek.Core;
using YerdenYuksek.Core.Caching;
using YerdenYuksek.Core.Domain.Customers;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Web.Framework.Persistence.Services.Public;

public class CustomerService : ICustomerService
{
    #region Fields

    private readonly CustomerSettings _customerSettings;

    private readonly IEncryptionService _encryptionService;

    private readonly IRepository<Customer> _customerRepository;

    private readonly IRepository<CustomerRole> _customerRoleRepository;

    private readonly IStaticCacheManager _staticCacheManager;

    private readonly IWebHelper _webHelper;

    #endregion

    #region Constructure and Destructure

    public CustomerService(
        CustomerSettings customerSettings,
        IEncryptionService encryptionService,
        IRepository<Customer> customerRepository,
        IStaticCacheManager staticCacheManager,
        IRepository<CustomerRole> customerRoleRepository,
        IWebHelper webHelper)
    {
        _customerSettings = customerSettings;
        _customerRepository = customerRepository;
        _encryptionService = encryptionService;
        _staticCacheManager = staticCacheManager;
        _customerRoleRepository = customerRoleRepository;
        _webHelper = webHelper;
    }

    #endregion

    #region Public Methods

    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        var query = from c in _customerRepository.Table
                    orderby c.Id
                    where c.Email == email
                    select c;
        var customer = await query.FirstOrDefaultAsync();

        return customer;
    }

    public async Task<CustomerRole?> GetCustomerRoleByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var key = _staticCacheManager.PrepareKeyForDefaultCache(YerdenYuksekCustomerServicesDefaults.CustomerRolesByNameCacheKey, name);

        var query = from cr in _customerRoleRepository.Table
                    orderby cr.Id
                    where cr.Name == name
        select cr;

        var customerRole = await _staticCacheManager.GetAsync(key, async () => await query.FirstOrDefaultAsync());

        return customerRole;
    }

    public async Task<Result> RegisterCustomerAsync(string email, string password)
    {
        if (await GetCustomerByEmailAsync(email) is not null)
        {
            return Result.Failure(Error.Conflict(description: "Email is already registered!"));
        }
        
        var customer = Customer.Create(email);
        customer.SetIpAddress(_webHelper.GetCurrentIpAddress());
        var customerPassword = new CustomerPassword
        {
            CustomerId = customer.Id,
            PasswordFormatId = (int)_customerSettings.DefaultPasswordFormat,
            CreatedOnUtc = DateTime.UtcNow
        };

        switch (customerPassword.GetPasswordFormat())
        {
            case PasswordFormat.Clear:
                customerPassword.Password = password;
                break;
            case PasswordFormat.Encrypted:
                customerPassword.Password = _encryptionService.EncryptText(password);
                break;
            case PasswordFormat.Hashed:
                var saltKey = _encryptionService.CreateSaltKey(YerdenYuksekCustomerServicesDefaults.PasswordSaltKeySize);
                customerPassword.PasswordSalt = saltKey;
                customerPassword.Password = _encryptionService.CreatePasswordHash(password, saltKey, _customerSettings.HashedPasswordFormat);
                break;
        }

        customer.SetCustomerPassword(customerPassword);

        var registeredRole = await GetCustomerRoleByNameAsync(CustomerDefaults.RegisteredRoleName);
        if (registeredRole is null)
        {
            return Result.Failure(Error.Failure(description: "Related customer role not found!"));
        }

        customer.AddCustomerRole(registeredRole);
        await InsertCustomerAsync(customer);

        return Result.Success();
    }

    public async Task InsertCustomerAsync(Customer customer)
    {
        await _customerRepository.InsertAsync(customer);
    }

    #endregion
}
