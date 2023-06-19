using System.Text.Json.Serialization;

namespace YerdenYuksek.Core.Configuration;

public interface IConfig
{
    [JsonIgnore]
    string Name => GetType().Name;
}