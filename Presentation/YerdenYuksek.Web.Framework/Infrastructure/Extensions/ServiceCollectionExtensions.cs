using eCommerce.Infrastructure.Persistence.Primitives;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Framework.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    #region Public Methods

    public static IServiceCollection RegisterServiceCollections(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureApiBehaviorOptions()
            .AddServices()
            .AddEntityFramework(configuration);

        return services;
    }

    #endregion

    #region Methods

    private static IServiceCollection ConfigureApiBehaviorOptions(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        return services;
    }

    private static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("ConnectionString") ??
            throw new InvalidOperationException("ConnectionString not found.");

        ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);

        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseMySql(connectionString, serverVersion);
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }

    #endregion
}
