using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Customers;

public class CustomerRoleMapping : BaseEntity
{
    #region Public Properties

    public Guid CustomerId { get; set; }

    public Guid RoleId { get; set; }

    #endregion

    #region Constructure and Destructure

    private CustomerRoleMapping(Guid customerId, Guid roleId)
    {
        CustomerId = customerId;
        RoleId = roleId;
    }

    #endregion

    #region Public Methods

    public static CustomerRoleMapping Create(Guid customerId, Guid roleId) => new(customerId, roleId);

    #endregion
}