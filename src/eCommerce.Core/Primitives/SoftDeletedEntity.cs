using eCommerce.Core.Interfaces;

namespace eCommerce.Core.Primitives;

public class SoftDeletedEntity : BaseEntity, ISoftDeletedEntity
{
    #region Public Properties

    public bool Active { get; set; }

    public bool Deleted { get; set; }

    #endregion

    #region Public Methods

    public void ActivateEntity()
    {
        // Silinmiş entity activate edilemez!
        if (Deleted) return;

        Active = true;
    }

    public void DeActivateEntity() => Active = false;

    public void DeleteEntity()
    {
        Active = false;
        Deleted = true;
    }

    #endregion
}
