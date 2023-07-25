using LinqToDB.Mapping;

namespace eCommerce.Infrastructure.Persistence.Configurations;

public interface IMappingEntityAccessor
{
    CustomEntityDescriptor GetEntityDescriptor(Type entityType);

    MappingSchema GetMappingSchema();
}