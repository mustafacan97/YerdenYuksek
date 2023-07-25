using eCommerce.Core.Primitives;
using MediatR;

namespace eCommerce.Application.Features.Commands.Customers.InsertCustomer;

public sealed class InsertCustomerCommand : IRequest<Result>
{
    #region Constructure and Destructure

    private InsertCustomerCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }

    #endregion

    #region Public Methods

    public static InsertCustomerCommand Create(string email, string password) => new(email, password);

    #endregion

    #region Public Properties

    public string Email { get; private set; }

    public string Password { get; private set; }

    #endregion    
}
