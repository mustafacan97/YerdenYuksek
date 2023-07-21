using eCommerce.Core.Primitives;
using MediatR;

namespace eCommerce.Application.Features.Queries.Customers.GetCustomerByEmail;

public sealed record GetCustomerByEmailQuery : IRequest<Result<GetCustomerByEmailResponse>>
{
    #region Constructure and Destructure

    private GetCustomerByEmailQuery(string email)
    {
        Email = email;
    }

    #endregion

    #region Public Methods

    public static GetCustomerByEmailQuery Create(string Email) => new GetCustomerByEmailQuery(Email);

    #endregion

    #region Public Properties

    public string Email { get; private set; }

    #endregion
}
