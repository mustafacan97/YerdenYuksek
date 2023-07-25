using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Security;

public class PermissionRoleMapping : BaseEntity
{
    #region Constructure and Destructure

    private PermissionRoleMapping(Guid roleId, Guid permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    #endregion

    #region Public Properties

    public Guid RoleId { get; set; }

    public Guid PermissionId { get; set; }

    #endregion

    #region Public Methods

    public static PermissionRoleMapping Create(Guid roleId, Guid permissionId) => new(roleId, permissionId);

    #endregion
}