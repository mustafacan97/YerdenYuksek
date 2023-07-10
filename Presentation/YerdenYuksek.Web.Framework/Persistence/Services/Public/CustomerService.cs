using Microsoft.EntityFrameworkCore;
using YerdenYuksek.Application.Models.Customers;
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
        var customerPassword = new CustomerPassword
        {
            CustomerId = customer.Id,
            PasswordFormatId = (int)_customerSettings.DefaultPasswordFormat,
            PasswordSalt = saltKey,
            Password = _encryptionService.CreatePasswordHash(password, saltKey, _customerSettings.HashedPasswordFormat),
            CreatedOnUtc = DateTime.UtcNow,
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

    #endregion

    #region Queries

    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        var query = from c in _unitOfWork.GetRepository<Customer>().Table
                    orderby c.Id
                    where c.Email == email
                    select c;
        var customer = await query.FirstOrDefaultAsync();

        return customer;
    }

    public string GetCustomerFullName(Customer customer)
    {
        if (customer is null)
        {
            throw new ArgumentNullException(nameof(customer));
        }

        var firstName = customer.FirstName;
        var lastName = customer.LastName;

        if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
        {
            return $"{firstName} {lastName}";
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                return firstName;
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                return lastName;
            }
        }

        return string.Empty;
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
}
