using eCommerce.Core.Services.ScheduleTasks;
using eCommerce.Infrastructure.Persistence.DataProviders;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    #region Public Methods

    public static IApplicationBuilder RegisterApplicationBuilders(this IApplicationBuilder application)
    {
        application.RunMigrationsOnStartup();

        application.ApplicationServices.RunScheduleTasksOnStartup();

        return application;
    }

    #endregion

    #region Methods

    private static void RunMigrationsOnStartup(this IApplicationBuilder application)
    {
        using var scope = application.ApplicationServices.CreateScope();
        var dataProvider = scope.ServiceProvider.GetService<ICustomDataProvider>();

        if (dataProvider is null)
        {
            return;
        }

        dataProvider.CreateDatabase("utf8mb4_0900_ai_ci");

        UpdateDatabase(scope.ServiceProvider);
    }

    private static void UpdateDatabase(IServiceProvider serviceProvider) => serviceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();

    private async static void RunScheduleTasksOnStartup(this IServiceProvider services)
    {
        var taskScheduler = services.GetRequiredService<ITaskScheduler>();
        await taskScheduler.InitializeAsync();
        taskScheduler.StartScheduler();
    }

    #endregion
}
