using YerdenYuksek.Application.Models.Customers;
using YerdenYuksek.Core.Domain.Customers;

namespace YerdenYuksek.Application.Services.Public.Customers;

public partial interface ICustomerService
{
    #region Commands

    Task InsertCustomerAsync(Customer customer);

    Task<RegisterResponseModel> RegisterCustomerAsync(string email, string password);

    #endregion

    #region Queries

    Task<Customer?> GetCustomerByEmailAsync(string email);

    string GetCustomerFullName(Customer customer);

    Task<CustomerRole?> GetCustomerRoleByNameAsync(string name);

    #endregion
}