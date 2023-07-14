using eCommerce.Core.Caching;
using eCommerce.Core.Domain.Security;

namespace eCommerce.Application.Services.Customers;

public static class YerdenYuksekCustomerServicesDefaults
{
    #region Public Properties

    public static int PasswordSaltKeySize => 6;

    public static string DefaultHashedPasswordFormat => "SHA512";

    #endregion

    #region Caching defaults

    #region Customer

    public static CacheKey CustomerBySystemNameCacheKey => new("ecommerce.customer.bysystemname.{0}");

    public static CacheKey CustomerByGuidCacheKey => new("ecommerce.customer.byguid.{0}");

    #endregion

    #region Customer roles

    public static CacheKey CustomerRolesAllCacheKey => new("YerdenYuksek.customerrole.all.{0}", EntityCacheDefaults<Role>.AllPrefix);

    public static CacheKey CustomerRolesByNameCacheKey => new("YerdenYuksek.customerrole.name.{0}", CustomerRolesByNamePrefix);

    public static string CustomerRolesByNamePrefix => "YerdenYuksek.customerrole.name.";

    public static CacheKey CustomerRoleIdsCacheKey => new("YerdenYuksek.customer.customerrole.ids.{0}-{1}", CustomerCustomerRolesPrefix);

    public static CacheKey CustomerRolesCacheKey => new("YerdenYuksek.customer.customerrole.{0}-{1}", CustomerCustomerRolesByCustomerPrefix, CustomerCustomerRolesPrefix);

    public static string CustomerCustomerRolesPrefix => "YerdenYuksek.customer.customerrole.";

    public static string CustomerCustomerRolesByCustomerPrefix => "YerdenYuksek.customer.customerrole.{0}";

    #endregion

    #region Addresses

    public static CacheKey CustomerAddressesCacheKey => new("YerdenYuksek.customer.addresses.{0}", CustomerAddressesPrefix);

    public static CacheKey CustomerAddressCacheKey => new("YerdenYuksek.customer.addresses.{0}-{1}", CustomerAddressesByCustomerPrefix, CustomerAddressesPrefix);

    public static string CustomerAddressesPrefix => "YerdenYuksek.customer.addresses.";

    public static string CustomerAddressesByCustomerPrefix => "YerdenYuksek.customer.addresses.{0}";

    #endregion

    #region Customer password

    public static CacheKey CustomerPasswordLifetimeCacheKey => new("YerdenYuksek.customerpassword.lifetime.{0}");

    #endregion

    #endregion
}