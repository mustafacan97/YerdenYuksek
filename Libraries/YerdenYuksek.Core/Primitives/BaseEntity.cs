namespace YerdenYuksek.Core.Primitives;

public class BaseEntity
{
    #region Constructure and Destructure

    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    public BaseEntity(Guid id)
    {
        Id = id;
    }

    #endregion

    #region Public Properties

    public Guid Id { get; set; }

    #endregion
}