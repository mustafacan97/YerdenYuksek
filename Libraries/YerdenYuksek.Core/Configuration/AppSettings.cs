using System.Text.Json;
using System.Text.Json.Serialization;

namespace YerdenYuksek.Core.Configuration;

public class AppSettings
{
    #region Fields

    private readonly Dictionary<Type, IConfig> _configurations;

    #endregion

    #region Constructure and Desctructure

    public AppSettings(IList<IConfig>? configurations = null)
    {
        _configurations = configurations
            ?.OrderBy(config => config.Name)
            ?.ToDictionary(config => config.GetType(), config => config)
            ?? new Dictionary<Type, IConfig>();
    }

    #endregion
    
    #region Public Methods

    public TConfig Get<TConfig>() where TConfig : class, IConfig
    {
        if (_configurations[typeof(TConfig)] is not TConfig config)
        {
            throw new Exception($"No configuration with type '{typeof(TConfig)}' found");
        }

        return config;
    }

    public void Update(IList<IConfig> configurations)
    {
        foreach (var config in configurations)
        {
            _configurations[config.GetType()] = config;
        }
    }

    #endregion

    #region Properties

    [JsonExtensionData]
    public Dictionary<string, JsonElement> Configuration { get; set; }

    #endregion
}