namespace YerdenYuksek.Core.Primitives;

public interface IUnitOfWork : IDisposable
{
    void SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(params IUnitOfWork[] unitOfWorks);

    void Rollback();    

    IRepository<T> GetRepository<T>() where T : BaseEntity;
}
