using eCommerce.Core.Entities.Security;
using eCommerce.Core.Shared;

namespace eCommerce.Core.Services.Customers;

public static class CustomerDefaults
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

    public static CacheKey CustomerRolesAllCacheKey => new("ecommerce.customerrole.all.{0}", EntityCacheDefaults<Role>.AllPrefix);

    public static CacheKey CustomerRolesByNameCacheKey => new("ecommerce.customerrole.name.{0}", CustomerRolesByNamePrefix);

    public static string CustomerRolesByNamePrefix => "ecommerce.customerrole.name.";

    public static CacheKey CustomerRoleIdsCacheKey => new("ecommerce.customer.customerrole.ids.{0}-{1}", CustomerCustomerRolesPrefix);

    public static CacheKey CustomerRolesCacheKey => new("ecommerce.customer.customerrole.{0}-{1}", CustomerCustomerRolesByCustomerPrefix, CustomerCustomerRolesPrefix);

    public static string CustomerCustomerRolesPrefix => "ecommerce.customer.customerrole.";

    public static string CustomerCustomerRolesByCustomerPrefix => "ecommerce.customer.customerrole.{0}";

    #endregion

    #region Addresses

    public static CacheKey CustomerAddressesCacheKey => new("ecommerce.customer.addresses.{0}", CustomerAddressesPrefix);

    public static CacheKey CustomerAddressCacheKey => new("ecommerce.customer.addresses.{0}-{1}", CustomerAddressesByCustomerPrefix, CustomerAddressesPrefix);

    public static string CustomerAddressesPrefix => "ecommerce.customer.addresses.";

    public static string CustomerAddressesByCustomerPrefix => "ecommerce.customer.addresses.{0}";

    #endregion

    #region Customer password

    public static CacheKey CustomerPasswordLifetimeCacheKey => new("ecommerce.customerpassword.lifetime.{0}");

    #endregion

    #endregion
}