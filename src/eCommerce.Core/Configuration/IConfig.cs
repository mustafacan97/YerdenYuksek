using System.Text.Json.Serialization;

namespace eCommerce.Core.Configuration;

public interface IConfig
{
    [JsonIgnore]
    string Name => GetType().Name;
}