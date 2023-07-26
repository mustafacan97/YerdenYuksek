using eCommerce.Core.Entities.Customers;

namespace eCommerce.Core.Services.Customers;

public partial interface ICustomerService
{
    Task<Customer?> GetCustomerByEmailAsync(string email, bool includeDeleted = false);

    Task UpdateCustomerAsync(Customer customer);
}