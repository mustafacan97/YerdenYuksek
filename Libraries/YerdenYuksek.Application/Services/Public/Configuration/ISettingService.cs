using YerdenYuksek.Core.Configuration;
using YerdenYuksek.Core.Domain.Configuration;

namespace YerdenYuksek.Application.Services.Public.Configuration;

public interface ISettingService
{
    Task<IList<Setting>> GetAllSettingsAsync();

    Task<T> GetSettingByKeyAsync<T>(string key, T defaultValue = default);

    Task<T> LoadSettingAsync<T>() where T : ISettings, new();

    Task<ISettings> LoadSettingAsync(Type type);
}