using Microsoft.AspNetCore.Http;
using System.Net;

namespace YerdenYuksek.Core;

public class WebHelper : IWebHelper
{
    #region Fields  

    private readonly IHttpContextAccessor _httpContextAccessor;

    #endregion

    #region Constructure and Destructure

    public WebHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    #region Public Methods

    public string GetCurrentIpAddress()
    {
        if (!IsRequestAvailable())
        {
            return string.Empty;
        }

        if (_httpContextAccessor.HttpContext.Connection?.RemoteIpAddress is not IPAddress remoteIp)
        {
            return "";
        }

        if (remoteIp.Equals(IPAddress.IPv6Loopback))
        {
            return IPAddress.Loopback.ToString();
        }

        return remoteIp.MapToIPv4().ToString();
    }

    #endregion

    #region Methods

    private bool IsRequestAvailable()
    {
        if (_httpContextAccessor?.HttpContext == null)
        {
            return false;
        }

        try
        {
            if (_httpContextAccessor.HttpContext?.Request == null)
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    #endregion
}
