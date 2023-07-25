using eCommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Security.Claims;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Shared;

namespace eCommerce.Infrastructure.Concretes;

public class WorkContext : IWorkContext
{
    #region Fields

    private readonly IRepository<Language> _languageRepository;

    private readonly IRepository<Customer> _customerRepository;

    private readonly IHttpContextAccessor _httpContextAccessor;

    #endregion

    #region Constructure and Destructure

    public WorkContext(
        IHttpContextAccessor httpContextAccessor,
        IRepository<Language> languageRepository,
        IRepository<Customer> customerRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _languageRepository = languageRepository;
        _customerRepository = customerRepository;
    }

    #endregion

    #region Public Methods

    public async Task<Customer?> GetCurrentCustomerAsync()
    {
        var idendity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;

        if (idendity is not null)
        {
            var customerEmail = idendity.Claims.FirstOrDefault(q => q.Type == ClaimTypes.Email)?.Value;
            if (customerEmail is null)
            {
                return null;
            }

            return (await _customerRepository.GetAllAsync(
                func: q => q.Where(p => p.Email == customerEmail),
                getCacheKey: q => q.PrepareKey(EntityCacheDefaults<Customer>.ByEmailCacheKey, customerEmail))).FirstOrDefault();
        }

        return null;
    }

    public async Task<Language> GetWorkingLanguageAsync()
    {
        var requestCultureFeature = _httpContextAccessor.HttpContext?.Features.Get<IRequestCultureFeature>();
        if (requestCultureFeature is null || requestCultureFeature.RequestCulture is null)
        {

            return (await _languageRepository.GetAllAsync(q => q.Where(p => p.IsDefaultLanguage))).First();
        }

        var requestLanguage = (await _languageRepository.GetAllAsync(
            func: q => q.Where(p => p.LanguageCulture.Equals(requestCultureFeature.RequestCulture.Culture.Name, StringComparison.InvariantCultureIgnoreCase))))
            .First();

        if (requestLanguage is null)
        {
            return (await _languageRepository.GetAllAsync(q => q.Where(p => p.IsDefaultLanguage))).First();
        }

        return requestLanguage;
    }

    #endregion
}
