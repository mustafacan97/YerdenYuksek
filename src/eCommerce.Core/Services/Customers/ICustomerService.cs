using eCommerce.Core.Primitives;
using eCommerce.Core.Entities.Security;
using eCommerce.Core.Entities.Customers;

namespace eCommerce.Core.Services.Customers;

public partial interface ICustomerService
{
    #region Commands

    Task InsertCustomerAsync(Customer customer);

    Task<Result<Customer>> RegisterCustomerAsync(string email, string password);

    Task<Result> ValidateCustomerAsync(string email, string password);

    #endregion

    #region Queries

    Task<Customer?> GetCustomerByEmailAsync(string email, bool includeDeleted = false);

    Task<Role?> GetCustomerRoleByNameAsync(string name);

    #endregion
}