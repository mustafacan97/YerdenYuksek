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

    private readonly IUnitOfWork _unitOfWork;

    private readonly IWebHelper _webHelper;

    #endregion

    #region Constructure and Destructure

    public InsertCustomerHandler(
        IUnitOfWork unitOfWork,
        IWebHelper webHelper)
    {
        _unitOfWork = unitOfWork;
        _webHelper = webHelper;
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

        var registeredRole = await _unitOfWork.GetRepository<Role>().GetFirstOrDefaultAsync(
            predicate: q => q.Name == RoleDefaults.RegisteredRoleName,
            getCacheKey: q => q.PrepareKey(EntityCacheDefaults<Role>.RolesByNameCacheKey, RoleDefaults.RegisteredRoleName));
        
        if (registeredRole is null)
        {
            return Result<Customer>.Failure(Error.Failure(description: "Related customer role not found!"));
        }

        var customer = Customer.Create(command.Email, customerSecurity, registeredRole);

        await _unitOfWork.GetRepository<Customer>().InsertAsync(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    #endregion
}
