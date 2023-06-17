using Microsoft.AspNetCore.Builder;

namespace eCommerce.Framework.Infrastructure;

public static class ApplicationBuilderExtensions
{
    #region Public Methods

    public static IApplicationBuilder RegisterApplicationBuilders(this IApplicationBuilder application)
    {
        return application;
    }

    #endregion
}
