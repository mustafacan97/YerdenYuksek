using YerdenYuksek.Core.Domain.Localization;

namespace YerdenYuksek.Application.Services.Public.Localization;

public interface ILanguageService
{
    Task DeleteLanguageAsync(Language language);

    Task UpdateLanguageAsync(Language language);

    Task InsertLanguageAsync(Language language);

    Task<IList<Language>> GetAllLanguagesAsync(bool onlyActive = true);

    IList<Language> GetAllLanguages(bool onlyActive = true);

    Task<Language> GetDefaultLanguageAsync();

    Task<Language> GetLanguageByIdAsync(Guid languageId);

    string? GetTwoLetterIsoLanguageName(Language language);
}