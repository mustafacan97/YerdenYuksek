using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives;
using eCommerce.Core.Entities.Security;
using eCommerce.Core.Entities.Configuration.CustomSettings;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Shared;
using eCommerce.Core.Services.Customers;

namespace eCommerce.Infrastructure.Services.Customers;

public class CustomerService : ICustomerService
{
    #region Fields

    private readonly CustomerSettings _customerSettings;

    private readonly IWebHelper _webHelper;

    private readonly IRepository<Customer> _customerRepository;

    private readonly IRepository<CustomerSecurity> _customerSecurityRepository;

    private readonly IRepository<CustomerRoleMapping> _crmRepository;

    private readonly IRepository<Role> _roleRepository;

    #endregion

    #region Constructure and Destructure

    public CustomerService(
        CustomerSettings customerSettings,
        IWebHelper webHelper,
        IRepository<Customer> customerRepository,
        IRepository<CustomerSecurity> customerSecurityRepository,
        IRepository<CustomerRoleMapping> crmRepository,
        IRepository<Role> roleRepository)
    {
        _customerSettings = customerSettings;
        _webHelper = webHelper;
        _customerRepository = customerRepository;
        _customerSecurityRepository = customerSecurityRepository;
        _crmRepository = crmRepository;
        _roleRepository = roleRepository;
    }

    #endregion

    #region Public Methods

    public async Task<Customer?> GetCustomerByEmailAsync(string email, bool includeDeleted = false)
    {
        return await _customerRepository.GetFirstOrDefaultAsync(
           func: q => from c in q
                      where c.Email == email
                      join cs in _customerSecurityRepository.Table on c.Id equals cs.CustomerId
                      join crm in _crmRepository.Table on c.Id equals crm.CustomerId
                      join r in _roleRepository.Table on crm.RoleId equals r.Id
                      select c.SetCustomerSecurity(c.Id, cs).SetCustomerRole(r),
           getCacheKey: q => q.PrepareKey(EntityCacheDefaults<Customer>.ByEmailCacheKey, email));
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        await _customerSecurityRepository.UpdateAsync(customer.CustomerSecurity);
        await _customerRepository.UpdateAsync(customer);        
    }

    #endregion
}
