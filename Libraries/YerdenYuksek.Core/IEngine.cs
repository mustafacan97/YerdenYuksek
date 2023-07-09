using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace YerdenYuksek.Core;

public interface IEngine
{
    void ConfigureRequestPipeline(IApplicationBuilder application);

    T Resolve<T>(IServiceScope? scope = null) where T : class;

    object Resolve(Type type, IServiceScope? scope = null);

    object ResolveUnregistered(Type type);
}