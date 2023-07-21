using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives;
using eCommerce.Core.Shared;
using MediatR;

namespace eCommerce.Application.Features.Queries.Customers.GetCustomerByEmail;

public class GetCustomerByEmailHandler : IRequestHandler<GetCustomerByEmailQuery, Result<GetCustomerByEmailResponse>>
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Constructure and Destructure

    public GetCustomerByEmailHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Public Methods

    public async Task<Result<GetCustomerByEmailResponse>> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.GetRepository<Customer>().GetFirstOrDefaultAsync(
            predicate: x => x.Email == request.Email,
            getCacheKey: q => q.PrepareKey(EntityCacheDefaults<Customer>.ByEmailCacheKey, request.Email) );

        if (customer is null)
        {
            return Result<GetCustomerByEmailResponse>.Success();
        }

        GetCustomerByEmailResponse response = new()
        {
            Email = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName
        };
        
        return Result<GetCustomerByEmailResponse>.Success(response);
    }

    #endregion
}
