namespace eCommerce.Infrastructure.Persistence;

public interface ITempDataStorage<T> : IQueryable<T>, IDisposable, IAsyncDisposable where T : class
{
}