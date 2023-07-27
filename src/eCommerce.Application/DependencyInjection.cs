using eCommerce.Application.Behaviours;
using eCommerce.Application.Features.Queries.Customers.GetCustomerByEmailAndPassword;
using eCommerce.Core.Entities.Customers;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace eCommerce.Application;

public static class DependencyInjection
{
    #region Public Methods

    public static IServiceCollection AddApplicationProject(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly)
            .AddMapster();

        return services;
    }

    #endregion

    private static IServiceCollection AddMapster(this IServiceCollection services, Action<TypeAdapterConfig>? options = null)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config
            .ForType<Customer, GetCustomerByEmailAndPasswordResponse>()
            .Map(dest => dest.Roles, src => src.CustomerRoles.Select(q => q.Name));

        config.Scan(typeof(DependencyInjection).Assembly);

        options?.Invoke(config);

        services.AddSingleton(config);

        return services;
    }
}
