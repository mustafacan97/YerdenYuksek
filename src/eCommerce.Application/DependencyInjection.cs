using eCommerce.Application.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Application;

public static class DependencyInjection
{
    #region Public Methods

    public static IServiceCollection AddApplicationProject(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }

    #endregion
}
