using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Security;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives;
using eCommerce.Core.Services.Customers;
using eCommerce.Core.Services.Security;
using eCommerce.Core.Shared;
using MediatR;

namespace eCommerce.Application.Features.Commands.Customers.InsertCustomer;

public class InsertCustomerHandler : IRequestHandler<InsertCustomerCommand, Result>
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;

    private readonly IEncryptionService _encryptionService;

    private readonly IWebHelper _webHelper;

    #endregion

    #region Constructure and Destructure

    public InsertCustomerHandler(
        IUnitOfWork unitOfWork,
        IEncryptionService encryptionService,
        IWebHelper webHelper)
    {
        _unitOfWork = unitOfWork;
        _encryptionService = encryptionService;
        _webHelper = webHelper;
    }

    #endregion

    #region Public Methods

    public async Task<Result> Handle(InsertCustomerCommand command, CancellationToken cancellationToken)
    {
        var saltKey = EncryptionHelper.CreateSaltKey(CustomerDefaults.PasswordSaltKeySize);
        var customer = Customer.Create(command.Email);
        var customerPassword = new CustomerSecurity
        {
            CustomerId = customer.Id,
            PasswordSalt = saltKey,
            Password = EncryptionHelper.CreatePasswordHash(command.Password,
                                                           saltKey,
                                                           CustomerDefaults.DefaultHashedPasswordFormat),
            LastIpAddress = _webHelper.GetCurrentIpAddress(),
        };

        var registeredRole = await _unitOfWork.GetRepository<Role>().GetFirstOrDefaultAsync(
            predicate: q => q.Name == RoleDefaults.RegisteredRoleName,
            getCacheKey: q => q.PrepareKey(EntityCacheDefaults<Role>.RolesByNameCacheKey, RoleDefaults.RegisteredRoleName));
        
        if (registeredRole is null)
        {
            return Result<Customer>.Failure(Error.Failure(description: "Related customer role not found!"));
        }

        customer.SetCustomerSecurity(customerPassword);
        customer.SetCustomerRole(registeredRole);

        await _unitOfWork.GetRepository<Customer>().InsertAsync(customer);

        await _unitOfWork.SaveChangesAsync();

        return Result<Customer>.Success(customer);
    }

    #endregion
}
