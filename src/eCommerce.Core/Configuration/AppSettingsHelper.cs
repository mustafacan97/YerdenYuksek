using eCommerce.Core.Infrastructure;
using System.Text;
using System.Text.Json;
using YerdenYuksek.Core.Infrastructure;

namespace eCommerce.Core.Configuration;

public class AppSettingsHelper
{
    #region Public Methods

    public static AppSettings SaveAppSettings(
        IList<IConfig> configurations, 
        IYerdenYuksekFileProvider fileProvider, 
        bool overwrite = true)
    {
        if (configurations is null)
        {
            throw new ArgumentNullException(nameof(configurations));
        }
        
        var appSettings = Singleton<AppSettings>.Instance ?? new AppSettings();
        appSettings.Update(configurations);
        Singleton<AppSettings>.Instance = appSettings;

        var filePath = fileProvider.MapPath(ConfigurationDefaults.AppSettingsFilePath);
        var fileExists = fileProvider.FileExists(filePath);
        fileProvider.CreateFile(filePath);

        //get raw configuration parameters
        var configuration = JsonSerializer.Deserialize<AppSettings>(fileProvider.ReadAllText(filePath, Encoding.UTF8))
            ?.Configuration 
            ?? new();

        foreach (var config in configurations)
        {
            configuration[config.Name] = JsonSerializer.SerializeToElement(config, config.GetType());
        }
        
        appSettings.Configuration = configuration
            .OrderBy(q => q.Key)
            .ToDictionary(q => q.Key, q => q.Value);

        if (!fileExists || overwrite)
        {
            var text = JsonSerializer.Serialize(appSettings, new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                WriteIndented = true
            });

            fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }

        return appSettings;
    }

    #endregion
}