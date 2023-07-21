namespace eCommerce.Application.Features.Queries.Customers.GetCustomerByEmail;

public record GetCustomerByEmailResponse
{
    #region Public Properties

    public string Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    #endregion
}
