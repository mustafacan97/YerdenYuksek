using eCommerce.Core.Interfaces;

namespace eCommerce.Infrastructure.Concretes;

public class HttpBaseUrlAccessor : IHttpBaseUrlAccessor
{
    #region Constructure and Destructure

    private HttpBaseUrlAccessor(string siteUrlString)
    {
        SiteUrlString = siteUrlString;
    }

    #endregion

    #region Properties

    private string SiteUrlString { get; set; } = string.Empty;

    #endregion

    #region Public Methods

    public static HttpBaseUrlAccessor Create(string siteUrlString) => new(siteUrlString);

    #endregion

    #region Public Methods

    public string GetHttpsUrl() => SiteUrlString.Split(";").First(g => g.StartsWith("https://"));

    public string GetHttpUrl() => SiteUrlString.Split(";").First(g => g.StartsWith("http://"));

    #endregion
}
