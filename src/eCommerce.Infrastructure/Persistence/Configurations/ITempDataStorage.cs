namespace eCommerce.Infrastructure.Persistence.Configurations;

public interface ITempDataStorage<T> : IQueryable<T>, IDisposable, IAsyncDisposable where T : class
{
}