using YerdenYuksek.Core.Domain.Customers;
using eCommerce.Core.Domain.Localization;

namespace eCommerce.Core.Interfaces;

public interface IWorkContext
{
    Task<Customer?> GetCurrentCustomerAsync();

    Task<Language> GetWorkingLanguageAsync();
}
