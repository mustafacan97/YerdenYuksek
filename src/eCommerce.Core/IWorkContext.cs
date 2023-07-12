using YerdenYuksek.Core.Domain.Customers;
using YerdenYuksek.Core.Domain.Localization;

namespace YerdenYuksek.Core;

public interface IWorkContext
{
    Task<Customer?> GetCurrentCustomerAsync();

    Task<Language> GetWorkingLanguageAsync();
}
