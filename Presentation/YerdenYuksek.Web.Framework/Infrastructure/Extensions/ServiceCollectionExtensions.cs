using eCommerce.Infrastructure.Persistence.Primitives;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Reflection;
using YerdenYuksek.Application.Services.Public.Configuration;
using YerdenYuksek.Application.Services.Public.Customers;
using YerdenYuksek.Application.Services.Public.Localization;
using YerdenYuksek.Application.Services.Public.Messages;
using YerdenYuksek.Application.Services.Public.ScheduleTasks;
using YerdenYuksek.Application.Services.Public.Security;
using YerdenYuksek.Core;
using YerdenYuksek.Core.Caching;
using YerdenYuksek.Core.Configuration;
using YerdenYuksek.Core.Infrastructure;
using YerdenYuksek.Core.Primitives;
using YerdenYuksek.Web.Framework.Common;
using YerdenYuksek.Web.Framework.Persistence;
using YerdenYuksek.Web.Framework.Persistence.Services.Public;

namespace eCommerce.Framework.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    #region Public Methods

    public static IServiceCollection RegisterServiceCollections(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services
            .ConfigureApiBehaviorOptions()
            .ConfigureApplicationSettings(environment)
            .AddServices()
            .RegisterAllSettings()
            .AddEntityFramework(configuration)
            .AddFluentValidation();

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

    private static IServiceCollection ConfigureApplicationSettings(this IServiceCollection services, IHostEnvironment environment)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

        CommonHelper.DefaultFileProvider = new YerdenYuksekFileProvider(environment);

        var typeFinder = new TypeFinder(CommonHelper.DefaultFileProvider);
        Singleton<ITypeFinder>.Instance = typeFinder;
        services.AddSingleton<ITypeFinder>(typeFinder);

        var configurations = typeFinder
                .FindClassesOfType<IConfig>()
                .Select(configType => (IConfig)Activator.CreateInstance(configType))
                .ToList();

        var appSettings = AppSettingsHelper.SaveAppSettings(configurations, CommonHelper.DefaultFileProvider, true);
        services.AddSingleton(appSettings);

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
        });

        return services;
    }

    public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddUnitOfWork<ApplicationDbContext>();
        services.AddSingleton<ILocker, MemoryCacheLocker>();
        services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();

        //file provider
        services.AddScoped<IYerdenYuksekFileProvider, YerdenYuksekFileProvider>();

        //add accessor to HttpContext
        services.AddHttpContextAccessor();

        //web helper
        services.AddScoped<IWebHelper, WebHelper>();

        //work context
        services.AddScoped<IWorkContext, WorkContext>();

        //static cache manager
        services.AddTransient(typeof(IConcurrentCollection<>), typeof(ConcurrentTrie<>));
        services.AddSingleton<ICacheKeyManager, CacheKeyManager>();
        services.AddMemoryCache();
        services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();

        //services
        services.AddScoped<ISettingService, SettingService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<ILocalizationService, LocalizationService>();
        services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();
        services.AddScoped<IScheduleTaskService, ScheduleTaskService>();
        services.AddScoped<IQueuedEmailService, QueuedEmailService>();

        //register all settings
        services.RegisterAllSettings();

        return services;
    }

    private static IServiceCollection RegisterAllSettings(this IServiceCollection services)
    {
        var typeFinder = Singleton<ITypeFinder>.Instance;
        var settings = typeFinder.FindClassesOfType(typeof(ISettings), false).ToList();

        foreach (var setting in settings)
        {
            services.AddScoped(setting, serviceProvider =>
            {
                return serviceProvider.GetRequiredService<ISettingService>().LoadSettingAsync(setting).Result;
            });
        }
        
        return services;
    }

    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetEntryAssembly())
            .AddFluentValidationClientsideAdapters();

        return services;
    }

    #endregion
}
