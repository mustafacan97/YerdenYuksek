namespace eCommerce.Application.Features.Queries.Customers.GetCustomerByEmailAndPassword;

public sealed record GetCustomerByEmailAndPasswordResponse
{
    #region Public Properties

    public string Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public IEnumerable<string> Roles { get; set; }

    #endregion
}
