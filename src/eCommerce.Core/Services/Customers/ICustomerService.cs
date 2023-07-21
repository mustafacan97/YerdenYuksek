using eCommerce.Core.Primitives;
using eCommerce.Core.Entities.Customers;

namespace eCommerce.Core.Services.Customers;

public partial interface ICustomerService
{
    #region Commands

    Task<Result> ValidateCustomerAsync(string email, string password);

    #endregion

    #region Queries

    Task<Customer?> GetCustomerByEmailAsync(string email, bool includeDeleted = false);

    #endregion
}