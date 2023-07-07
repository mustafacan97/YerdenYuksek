using YerdenYuksek.Core.Domain.Customers;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Application.Services.Public.Customers;

public partial interface ICustomerService
{
    Task<Customer?> GetCustomerByEmailAsync(string email);

    Task<Customer> GetCustomerByIdAsync(Guid customerId);

    Task<string> GetCustomerFullNameAsync(Customer customer);

    Task<Result> RegisterCustomerAsync(string email, string password);

    Task<CustomerRole?> GetCustomerRoleByNameAsync(string name);

    Task InsertCustomerAsync(Customer customer);
}