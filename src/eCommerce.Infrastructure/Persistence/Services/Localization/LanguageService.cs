using System.Globalization;
using eCommerce.Application.Services.Configuration;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Caching;
using eCommerce.Application.Services.Localization;
using eCommerce.Core.Entities.Configuration.CustomSettings;
using eCommerce.Core.Entities.Localization;

namespace eCommerce.Infrastructure.Persistence.Services.Localization;

public class LanguageService : ILanguageService
{
    #region Fields

    private readonly LocalizationSettings _localizationSettings;

    private readonly ISettingService _settingService;

    private readonly IStaticCacheManager _staticCacheManager;

    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Constructure and Destructure

    public LanguageService(
        LocalizationSettings localizationSettings,
        ISettingService settingService,
        IStaticCacheManager staticCacheManager,
        IUnitOfWork unitOfWork)
    {
        _localizationSettings = localizationSettings;
        _settingService = settingService;
        _staticCacheManager = staticCacheManager;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Public Methods

    #region Commands

    public async Task DeleteLanguageAsync(Language language)
    {
        if (language is null)
        {
            throw new ArgumentNullException(nameof(language));
        }

        if (_localizationSettings.DefaultLanguageId == language.Id)
        {
            foreach (var activeLanguage in await GetAllLanguagesAsync())
            {
                if (activeLanguage.Id == language.Id)
                {
                    continue;
                }

                _localizationSettings.DefaultLanguageId = activeLanguage.Id;
                await _settingService.SaveSettingAsync(_localizationSettings);
                break;
            }
        }

        _unitOfWork.GetRepository<Language>().Delete(language);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task InsertLanguageAsync(Language language)
    {
        await _unitOfWork.GetRepository<Language>().InsertAsync(language);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateLanguageAsync(Language language)
    {
        _unitOfWork.GetRepository<Language>().Update(language);
        await _unitOfWork.SaveChangesAsync();
    }

    #endregion

    #region Queries

    public IList<Language> GetAllLanguages(bool onlyActive = true)
    {
        var key = _staticCacheManager.PrepareKeyForDefaultCache(LocalizationDefaults.LanguagesAllCacheKey, onlyActive);

        var languages = _staticCacheManager.Get(key, () =>
        {
            var allLanguages = _unitOfWork.GetRepository<Language>().GetAll(query =>
            {
                if (onlyActive)
                {
                    query = query.Where(l => l.Active && !l.Deleted);
                }

                query = query.OrderBy(l => l.DisplayOrder).ThenBy(l => l.Id);

                return query;
            });

            return allLanguages;
        });

        return languages;
    }

    public async Task<IList<Language>> GetAllLanguagesAsync(bool onlyActive = true)
    {
        var key = _staticCacheManager.PrepareKeyForDefaultCache(LocalizationDefaults.LanguagesAllCacheKey, onlyActive);

        var languages = await _staticCacheManager.GetAsync(key, async () =>
        {
            var allLanguages = await _unitOfWork.GetRepository<Language>().GetAllAsync(query =>
            {
                if (onlyActive)
                {
                    query = query.Where(l => l.Active && !l.Deleted);
                }

                query = query.OrderBy(l => l.DisplayOrder).ThenBy(l => l.Id);

                return query;
            });

            return allLanguages;
        });

        return languages;
    }

    public async Task<Language> GetDefaultLanguageAsync()
    {
        return (await _unitOfWork.GetRepository<Language>().GetFirstOrDefaultAsync<Language>(
            q => q.IsDefaultLanguage && q.Active && !q.Deleted))!;
    }

    public async Task<Language> GetLanguageByIdAsync(Guid languageId)
    {
        return await _unitOfWork.GetRepository<Language>().GetByIdAsync(languageId, cache => default);
    }

    public string? GetTwoLetterIsoLanguageName(Language language)
    {
        if (language is null)
        {
            return null;
        }

        if (string.IsNullOrEmpty(language.LanguageCulture))
        {
            return null;
        }

        var culture = new CultureInfo(language.LanguageCulture);
        var code = culture.TwoLetterISOLanguageName;

        return string.IsNullOrEmpty(code) ? null : code;
    }

    #endregion

    #endregion
}