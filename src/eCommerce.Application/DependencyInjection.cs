using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Application;

public static class DependencyInjection
{
    #region Public Methods

    public static IServiceCollection AddApplicationProject(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }

    #endregion
}
