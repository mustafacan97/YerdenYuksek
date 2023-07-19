using eCommerce.Core.Services.ScheduleTasks;
using eCommerce.Infrastructure.Concretes;
using eCommerce.Infrastructure.Persistence.Primitives;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Infrastructure.Infrastructure;

public static class ApplicationBuilderExtensions
{
    #region Public Methods

    public static IApplicationBuilder RegisterApplicationBuilders(this IApplicationBuilder application)
    {
        application.ConfigureRequestPipeline();

        application.ApplicationServices.RunMigrationsOnStartup();

        application.ApplicationServices.RunScheduleTasksOnStartup();

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

    public static void ConfigureRequestPipeline(this IApplicationBuilder application)
    {
        EngineContext.Current.ConfigureRequestPipeline(application);
    }

    private async static void RunScheduleTasksOnStartup(this IServiceProvider services)
    {
        var taskScheduler = services.GetRequiredService<ITaskScheduler>();
        await taskScheduler.InitializeAsync();
        taskScheduler.StartScheduler();
    }

    #endregion
}
