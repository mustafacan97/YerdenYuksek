using System.Globalization;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Entities.Configuration.CustomSettings;
using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Services.Caching;
using eCommerce.Core.Services.Configuration;
using eCommerce.Core.Services.Localization;

namespace eCommerce.Infrastructure.Services.Localization;

public class LanguageService : ILanguageService
{
    #region Fields

    private readonly LocalizationSettings _localizationSettings;

    private readonly ISettingService _settingService;

    private readonly IStaticCacheManager _staticCacheManager;

    private readonly IRepository<Language> _languageRepository;

    #endregion

    #region Constructure and Destructure

    public LanguageService(
        LocalizationSettings localizationSettings,
        ISettingService settingService,
        IStaticCacheManager staticCacheManager,
        IRepository<Language> languageRepository)
    {
        _localizationSettings = localizationSettings;
        _settingService = settingService;
        _staticCacheManager = staticCacheManager;
        _languageRepository = languageRepository;
    }

    #endregion

    #region Public Methods

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

        await _languageRepository.DeleteAsync(language);
    }

    public async Task InsertLanguageAsync(Language language) => await _languageRepository.InsertAsync(language);

    public async Task UpdateLanguageAsync(Language language) => await _languageRepository.UpdateAsync(language);

    public IList<Language> GetAllLanguages(bool onlyActive = true)
    {
        return _languageRepository.GetAll(getCacheKey: q => q.PrepareKeyForDefaultCache(LocalizationDefaults.LanguagesAllCacheKey, onlyActive));
    }

    public async Task<IList<Language>> GetAllLanguagesAsync(bool onlyActive = true)
    {
        return await _languageRepository.GetAllAsync(getCacheKey: q => q.PrepareKeyForDefaultCache(LocalizationDefaults.LanguagesAllCacheKey, onlyActive));
    }

    public async Task<Language> GetDefaultLanguageAsync()
    {
        return (await _languageRepository.GetFirstOrDefaultAsync(q => q.Where(p => p.IsDefaultLanguage)))!;
    }

    public async Task<Language> GetLanguageByIdAsync(Guid languageId) => await _languageRepository.GetByIdAsync(languageId);

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
}