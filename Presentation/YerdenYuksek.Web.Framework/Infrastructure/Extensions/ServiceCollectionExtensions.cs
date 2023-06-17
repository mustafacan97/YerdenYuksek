using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Framework.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    #region Public Methods

    public static IServiceCollection RegisterServiceCollections(this IServiceCollection services)
    {
        return services;
    }

    #endregion
}
