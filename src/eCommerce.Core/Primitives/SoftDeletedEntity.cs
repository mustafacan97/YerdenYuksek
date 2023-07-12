using eCommerce.Core.Interfaces;
using YerdenYuksek.Core.Primitives;

namespace eCommerce.Core.Primitives;

public class SoftDeletedEntity : BaseEntity, ISoftDeletedEntity
{
    #region Public Properties

    public bool IsActive { get; set; }

    public bool Deleted { get; set; }

    #endregion

    #region Public Methods

    public void ActivateEntity()
    {
        // Silinmiş entity activate edilemez!
        if (Deleted) return;

        IsActive = true;
    }

    public void DeActivateEntity() => IsActive = false;

    public void DeleteEntity()
    {
        IsActive = false;
        Deleted = true;
    }

    #endregion
}
