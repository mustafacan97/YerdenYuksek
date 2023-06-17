using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Stores;

public class StoreMapping : BaseEntity
{
    #region Constructure and Destructure

    public StoreMapping()
    {
    }

    #endregion

    #region Public Properties

    public Guid EntityId { get; set; }

    public string EntityName { get; set; }

    public Guid StoreId { get; set; }

    #endregion
}
