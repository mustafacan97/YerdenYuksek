using eCommerce.Infrastructure.Persistence.Primitives;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YerdenYuksek.Web.Framework.Persistence;
using eCommerce.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using eCommerce.Core.Services.Caching;
using eCommerce.Infrastructure.Services.Caching;
using eCommerce.Core.Services.Security;
using eCommerce.Core.Shared;
using eCommerce.Infrastructure.Services.ScheduleTasks;
using eCommerce.Core.Services.ScheduleTasks;
using eCommerce.Infrastructure.Services.Messages;
using eCommerce.Core.Services.Messages;
using eCommerce.Infrastructure.Services.Secuirty;
using eCommerce.Infrastructure.Services.Configuration;
using eCommerce.Core.Services.Configuration;
using eCommerce.Infrastructure.Services.Customers;
using eCommerce.Core.Services.Customers;
using eCommerce.Infrastructure.Services.Localization;
using eCommerce.Core.Services.Localization;
using eCommerce.Infrastructure.Concretes;
using eCommerce.Infrastructure.Infrastructure;

namespace eCommerce.Infrastructure.Infrastructure;

public static class ServiceCollectionExtensions
{
    #region Public Methods

    public static IServiceCollection AddInfrastructureProject(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .ConfigureApiBehaviorOptions()
            .AddJwtBearer(configuration)
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

        //email
        services.AddScoped<ITokenizer, Tokenizer>();
        services.AddScoped<IEmailSender, EmailSender>();

        //services
        services.AddScoped<ISettingService, SettingService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<ILocalizationService, LocalizationService>();
        services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();
        services.AddScoped<IScheduleTaskService, ScheduleTaskService>();
        services.AddScoped<IQueuedEmailService, QueuedEmailService>();
        services.AddScoped<IWorkflowMessageService, WorkflowMessageService>();
        services.AddScoped<IEmailAccountService, EmailAccountService>();
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();
        services.AddScoped<IMessageTokenProvider, MessageTokenProvider>();
        services.AddScoped<IJwtService, JwtService>();

        //schedule tasks
        services.AddSingleton<ITaskScheduler, Services.ScheduleTasks.TaskScheduler>();
        services.AddTransient<IScheduleTaskRunner, ScheduleTaskRunner>();

        return services;
    }

    private static IServiceCollection RegisterAllSettings(this IServiceCollection services)
    {
        var settings = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(ISettings).IsAssignableFrom(p) && !p.IsInterface);

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

    private static IServiceCollection AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey = configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new Exception("JWT Key Not Found!");
        }

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var Key = Encoding.UTF8.GetBytes(jwtKey);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Key)
            };
        });

        return services;
    }

    #endregion
}
