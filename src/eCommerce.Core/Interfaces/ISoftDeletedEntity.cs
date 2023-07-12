namespace eCommerce.Core.Interfaces;

public interface ISoftDeletedEntity
{
    #region Public Properties

    bool IsActive { get; set; }

    bool Deleted { get; set; }

    #endregion

    #region Public Methods

    void ActivateEntity();

    void DeActivateEntity();

    void DeleteEntity();

    #endregion
}
