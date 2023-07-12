namespace YerdenYuksek.Core.Primitives;

public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : class
{
    TContext DbContext { get; }    
}
