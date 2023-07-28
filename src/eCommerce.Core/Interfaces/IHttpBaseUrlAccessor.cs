namespace eCommerce.Core.Interfaces;

public interface IHttpBaseUrlAccessor
{
    string GetHttpsUrl();

    string GetHttpUrl();
}
