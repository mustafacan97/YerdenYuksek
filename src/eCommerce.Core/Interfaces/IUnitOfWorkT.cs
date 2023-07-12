namespace eCommerce.Core.Interfaces;

public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : class
{
    TContext DbContext { get; }
}
