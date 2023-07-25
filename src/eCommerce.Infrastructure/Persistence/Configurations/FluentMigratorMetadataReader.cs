using System.Collections.Concurrent;
using System.Reflection;
using eCommerce.Core.Primitives;
using FluentMigrator.Expressions;
using LinqToDB.Mapping;
using LinqToDB.Metadata;

namespace eCommerce.Infrastructure.Persistence.Configurations;

public class FluentMigratorMetadataReader : IMetadataReader
{
    #region Fields

    protected readonly IMappingEntityAccessor _mappingEntityAccessor;

    #endregion

    #region Properties

    protected static ConcurrentDictionary<(Type, MemberInfo), Attribute> Types { get; } = new ConcurrentDictionary<(Type, MemberInfo), Attribute>();

    protected static ConcurrentDictionary<Type, CreateTableExpression> Expressions { get; } = new ConcurrentDictionary<Type, CreateTableExpression>();

    #endregion

    #region Constructure and Destructure

    public FluentMigratorMetadataReader(IMappingEntityAccessor mappingEntityAccessor)
    {
        _mappingEntityAccessor = mappingEntityAccessor;
    }

    #endregion    

    #region Public Methods

    public T[] GetAttributes<T>(Type type, bool inherit = true) where T : Attribute => GetAttributes<T>(type, typeof(TableAttribute));

    public T[] GetAttributes<T>(Type type, MemberInfo memberInfo, bool inherit = true) where T : Attribute => GetAttributes<T>(type, typeof(ColumnAttribute), memberInfo);

    public MemberInfo[] GetDynamicColumns(Type type) => Array.Empty<MemberInfo>();

    #endregion

    #region Methods

    protected T GetAttribute<T>(Type type, MemberInfo memberInfo) where T : Attribute
    {
        var attribute = Types.GetOrAdd((type, memberInfo), _ =>
        {
            var entityDescriptor = _mappingEntityAccessor.GetEntityDescriptor(type);

            if (typeof(T) == typeof(TableAttribute))
                return new TableAttribute(entityDescriptor.EntityName) { Schema = entityDescriptor.SchemaName };

            if (typeof(T) != typeof(ColumnAttribute))
                return null;

            var entityField = entityDescriptor.Fields.SingleOrDefault(cd => cd.Name.Equals(memberInfo.Name, StringComparison.OrdinalIgnoreCase));

            if (entityField is null)
                return null;

            if (!(memberInfo as PropertyInfo)?.CanWrite ?? false)
                return null;

            var columnSystemType = (memberInfo as PropertyInfo)?.PropertyType ?? typeof(string);

            var mappingSchema = _mappingEntityAccessor.GetMappingSchema();

            return new ColumnAttribute
            {
                Name = entityField.Name,
                IsPrimaryKey = entityField.IsPrimaryKey,
                IsColumn = true,
                CanBeNull = entityField.IsNullable ?? false,
                Length = entityField.Size ?? 0,
                Precision = entityField.Precision ?? 0,
                IsIdentity = entityField.IsIdentity,
                DataType = mappingSchema.GetDataType(columnSystemType).Type.DataType
            };
        });

        return (T)attribute;
    }

    protected T[] GetAttributes<T>(Type type, Type attributeType, MemberInfo? memberInfo = null) where T : Attribute
    {
        if (type.IsSubclassOf(typeof(BaseEntity)) && typeof(T) == attributeType && GetAttribute<T>(type, memberInfo) is T attr)
        {
            return new[] { attr };
        }

        return Array.Empty<T>();
    }

    #endregion    
}
