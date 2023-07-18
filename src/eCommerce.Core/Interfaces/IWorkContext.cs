using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Localization;

namespace eCommerce.Core.Interfaces;

public interface IWorkContext
{
    Task<Customer?> GetCurrentCustomerAsync();

    Task<Language> GetWorkingLanguageAsync();
}
