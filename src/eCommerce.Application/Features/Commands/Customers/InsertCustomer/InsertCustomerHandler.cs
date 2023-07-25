using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Security;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives;
using eCommerce.Core.Services.Customers;
using eCommerce.Core.Shared;
using MediatR;

namespace eCommerce.Application.Features.Commands.Customers.InsertCustomer;

public class InsertCustomerHandler : IRequestHandler<InsertCustomerCommand, Result>
{
    #region Fields

    private readonly IRepository<Customer> _customerRepository;

    private readonly IRepository<Role> _roleRepository;

    private readonly IRepository<CustomerRoleMapping> _crmRepository;

    private readonly IWebHelper _webHelper;

    #endregion

    #region Constructure and Destructure

    public InsertCustomerHandler(
        IWebHelper webHelper,
        IRepository<Role> roleRepository,
        IRepository<Customer> customerRepository,
        IRepository<CustomerRoleMapping> crmRepository)
    {
        _webHelper = webHelper;
        _roleRepository = roleRepository;
        _customerRepository = customerRepository;
        _crmRepository = crmRepository;
    }

    #endregion

    #region Public Methods

    public async Task<Result> Handle(InsertCustomerCommand command, CancellationToken cancellationToken)
    {
        var saltKey = EncryptionHelper.CreateSaltKey(CustomerDefaults.PasswordSaltKeySize);
        var customerSecurity = new CustomerSecurity
        {
            PasswordSalt = saltKey,
            Password = EncryptionHelper.CreatePasswordHash(command.Password,
                                                           saltKey,
                                                           CustomerDefaults.DefaultHashedPasswordFormat),
            LastIpAddress = _webHelper.GetCurrentIpAddress(),
        };

        var registeredRole = (await _roleRepository.GetAllAsync(
            func: q => q.Where(p => p.Name == RoleDefaults.RegisteredRoleName),
            getCacheKey: q => q.PrepareKey(EntityCacheDefaults<Role>.RolesByNameCacheKey, RoleDefaults.RegisteredRoleName))).FirstOrDefault();
        
        if (registeredRole is null)
        {
            return Result<Customer>.Failure(Error.Failure(description: "Related customer role not found!"));
        }

        var customer = Customer.Create(command.Email, customerSecurity, registeredRole);
        var crm = CustomerRoleMapping.Create(customer.Id, registeredRole.Id);

        await _customerRepository.InsertAsync(customer);
        await _crmRepository.InsertAsync(crm);

        return Result.Success();
    }

    #endregion
}
