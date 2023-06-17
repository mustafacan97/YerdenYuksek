namespace YerdenYuksek.Core.Primitives;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    #region Properties

    IQueryable<TEntity> Table { get; }

    #endregion
}