using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Security.Claims;
using YerdenYuksek.Application.Services.Public.Customers;
using YerdenYuksek.Application.Services.Public.Localization;
using YerdenYuksek.Core;
using YerdenYuksek.Core.Domain.Customers;
using YerdenYuksek.Core.Domain.Localization;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Web.Framework.Common;

public class WorkContext : IWorkContext
{
    #region Fields

    private readonly ICustomerService _customerService;

    private readonly ILanguageService _languageService;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IHttpContextAccessor _httpContextAccessor;

    #endregion

    #region Constructure and Destructure

    public WorkContext(
        ILanguageService languageService,
        IHttpContextAccessor httpContextAccessor,
        ICustomerService customerService,
        IUnitOfWork unitOfWork)
    {
        _languageService = languageService;
        _httpContextAccessor = httpContextAccessor;
        _customerService = customerService;
        _unitOfWork = unitOfWork;
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

            return await _customerService.GetCustomerByEmailAsync(customerEmail);
        }

        return null;
    }

    public async Task<Language> GetWorkingLanguageAsync()
    {
        var requestCultureFeature = _httpContextAccessor.HttpContext?.Features.Get<IRequestCultureFeature>();
        if (requestCultureFeature is null || requestCultureFeature.RequestCulture is null)
        {
            return await _languageService.GetDefaultLanguageAsync();
        }

        var requestLanguage = await _unitOfWork.GetRepository<Language>().GetFirstOrDefaultAsync<Language>(l =>
            l.LanguageCulture.Equals(requestCultureFeature.RequestCulture.Culture.Name, StringComparison.InvariantCultureIgnoreCase) &&
            l.Active && !l.Deleted, false);
        if (requestLanguage is null)
        {
            return await _languageService.GetDefaultLanguageAsync();
        }

        return requestLanguage;
    }

    #endregion
}
