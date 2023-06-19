using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Configuration;

public class Setting : BaseEntity
{
    #region Constructure and Destructure

    public Setting()
    {
    }

    public Setting(string name, string value, Guid storeId)
    {
        Name = name;
        Value = value;
        StoreId = storeId;
    }

    #endregion

    #region Public Properties

    public string Name { get; set; }

    public string Value { get; set; }

    public Guid StoreId { get; set; }

    #endregion

    #region Public Methods

    public override string ToString()
    {
        return Name;
    }

    #endregion
}
