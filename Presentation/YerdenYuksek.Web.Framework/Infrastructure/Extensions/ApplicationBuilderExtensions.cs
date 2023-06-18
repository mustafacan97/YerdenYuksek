using eCommerce.Infrastructure.Persistence.Primitives;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Framework.Infrastructure;

public static class ApplicationBuilderExtensions
{
    #region Public Methods

    public static IApplicationBuilder RegisterApplicationBuilders(this IApplicationBuilder application)
    {
        application.ApplicationServices.RunMigrationsOnStartup();

        return application;
    }

    #endregion

    #region Methods

    private static void RunMigrationsOnStartup(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }

    #endregion
}
