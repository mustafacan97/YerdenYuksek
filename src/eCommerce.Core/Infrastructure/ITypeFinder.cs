using System.Reflection;

namespace eCommerce.Core.Infrastructure;

public interface ITypeFinder
{
    IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

    IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

    IList<Assembly> GetAssemblies();
}
