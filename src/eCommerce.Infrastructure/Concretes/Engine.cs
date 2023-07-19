using eCommerce.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Infrastructure.Concretes;

public class Engine : IEngine
{
    #region Public Properties

    public IServiceProvider ServiceProvider { get; protected set; }

    #endregion

    #region Public Methods

    public void ConfigureRequestPipeline(IApplicationBuilder application)
    {
        ServiceProvider = application.ApplicationServices;
    }

    public T Resolve<T>(IServiceScope? scope = null) where T : class
    {
        return (T)Resolve(typeof(T), scope);
    }

    public object Resolve(Type type, IServiceScope? scope = null)
    {
        return GetServiceProvider(scope)?.GetService(type);
    }

    public object ResolveUnregistered(Type type)
    {
        Exception? innerException = null;
        foreach (var constructor in type.GetConstructors())
        {
            try
            {
                //try to resolve constructor parameters
                var parameters = constructor.GetParameters().Select(parameter =>
                {
                    var service = Resolve(parameter.ParameterType);
                    if (service is null)
                    {
                        throw new Exception("Unknown dependency");
                    }

                    return service;
                });

                //all is ok, so create instance
                return Activator.CreateInstance(type, parameters.ToArray());
            }
            catch (Exception ex)
            {
                innerException = ex;
            }
        }

        throw new Exception("No constructor was found that had all the dependencies satisfied.", innerException);
    }

    #endregion

    #region Methods

    private IServiceProvider GetServiceProvider(IServiceScope? scope = null)
    {
        if (scope == null)
        {
            var accessor = ServiceProvider?.GetService<IHttpContextAccessor>();
            var context = accessor?.HttpContext;
            return context?.RequestServices ?? ServiceProvider;
        }
        return scope.ServiceProvider;
    }

    #endregion
}