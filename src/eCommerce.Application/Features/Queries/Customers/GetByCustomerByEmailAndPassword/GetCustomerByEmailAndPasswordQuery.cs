using eCommerce.Core.Primitives;
using MediatR;

namespace eCommerce.Application.Features.Queries.Customers.GetCustomerByEmailAndPassword;

public sealed record GetCustomerByEmailAndPasswordQuery : IRequest<Result<GetCustomerByEmailAndPasswordResponse>>
{
    #region Constructure and Destructure

    private GetCustomerByEmailAndPasswordQuery(string email, string password)
    {
        Email = email;
        Password = password;
    }

    #endregion

    #region Public Methods

    public static GetCustomerByEmailAndPasswordQuery Create(string Email, string Password) => new(Email, Password);

    #endregion

    #region Public Properties

    public string Email { get; private set; }

    public string Password { get; private set; }

    #endregion
}
