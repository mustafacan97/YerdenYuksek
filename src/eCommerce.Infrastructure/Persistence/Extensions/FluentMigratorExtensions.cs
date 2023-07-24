using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Persistence.Builders;
using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Infrastructure.Extensions;
using LinqToDB.Mapping;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace eCommerce.Infrastructure.Persistence.Extensions;

public static class FluentMigratorExtensions
{
    #region Static Fields and Constants

    private const int DATE_TIME_PRECISION = 6;

    private static Dictionary<Type, Action<ICreateTableColumnAsTypeSyntax>> TypeMapping { get; } = new Dictionary<Type, Action<ICreateTableColumnAsTypeSyntax>>
    {
        [typeof(int)] = c => c.AsInt32(),
        [typeof(long)] = c => c.AsInt64(),
        [typeof(string)] = c => c.AsString(int.MaxValue).Nullable(),
        [typeof(bool)] = c => c.AsBoolean(),
        [typeof(decimal)] = c => c.AsDecimal(18, 4),
        [typeof(DateTime)] = c => c.AsCustom($"datetime({DATE_TIME_PRECISION})"),
        [typeof(byte[])] = c => c.AsBinary(int.MaxValue),
        [typeof(Guid)] = c => c.AsGuid()
    };

    #endregion

    #region Public Methods

    public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax ForeignKey<TPrimary>(
        this ICreateTableColumnOptionOrWithColumnSyntax column,
        string? primaryTableName = null,
        string? primaryColumnName = null,
        Rule onDelete = Rule.Cascade) where TPrimary : BaseEntity
    {
        if (string.IsNullOrEmpty(primaryTableName))
        {
            primaryTableName = typeof(TPrimary).Name;
        }

        if (string.IsNullOrEmpty(primaryColumnName))
        {
            primaryColumnName = nameof(BaseEntity.Id);
        }

        return column.Indexed().ForeignKey(primaryTableName, primaryColumnName).OnDelete(onDelete);
    }

    public static IAlterTableColumnOptionOrAddColumnOrAlterColumnOrForeignKeyCascadeSyntax ForeignKey<TPrimary>(
        this IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax column,
        string? primaryTableName = null,
        string? primaryColumnName = null,
        Rule onDelete = Rule.Cascade) where TPrimary : BaseEntity
    {
        if (string.IsNullOrEmpty(primaryTableName))
        {
            primaryTableName = typeof(TPrimary).Name;
        }

        if (string.IsNullOrEmpty(primaryColumnName))
        {
            primaryColumnName = nameof(BaseEntity.Id);
        }

        return column.Indexed().ForeignKey(primaryTableName, primaryColumnName).OnDelete(onDelete);
    }

    public static void TableFor<TEntity>(this ICreateExpressionRoot expressionRoot) where TEntity : BaseEntity
    {
        if (expressionRoot.Table(typeof(TEntity).Name) is CreateTableExpressionBuilder builder)
        {
            builder.RetrieveTableExpressions(typeof(TEntity));
        }
    }

    public static void RetrieveTableExpressions(this CreateTableExpressionBuilder builder, Type type)
    {
        var selectedBuilder = Assembly.GetAssembly(typeof(IEntityBuilder))!
            .GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IEntityBuilder)) && t.IsClass && !t.IsAbstract && !t.IsInterface)
            .FirstOrDefault();

        if (selectedBuilder is null)
        {
            return;
        }

        (Activator.CreateInstance(selectedBuilder) as IEntityBuilder)?.MapEntity(builder);

        var propertiesToAutoMap = type
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
            .Where(pi => pi.DeclaringType != typeof(BaseEntity) &&
            pi.CanWrite &&
            !pi.HasAttribute<NotMappedAttribute>() && !pi.HasAttribute<NotColumnAttribute>() &&
            !builder.Expression.Columns.Any(x => x.Name.Equals(pi.Name, StringComparison.OrdinalIgnoreCase)) &&
            TypeMapping.ContainsKey(GetTypeToMap(pi.PropertyType).propType));

        foreach (var prop in propertiesToAutoMap)
        {
            var columnName = prop.Name;
            var (propType, canBeNullable) = GetTypeToMap(prop.PropertyType);
            DefineByOwnType(columnName, propType, builder, canBeNullable);
        }
    }    

    public static (Type propType, bool canBeNullable) GetTypeToMap(this Type type)
    {
        return Nullable.GetUnderlyingType(type) is Type uType ?
            (uType, true) :
            (type, false);
    }

    #endregion

    #region Methods

    private static void DefineByOwnType(string? columnName, Type propType, CreateTableExpressionBuilder create, bool canBeNullable = false)
    {
        if (string.IsNullOrEmpty(columnName))
        {
            throw new ArgumentException("The column name cannot be empty");
        }

        if (propType == typeof(string) || propType.FindInterfaces((t, o) => t.FullName?.Equals(o.ToString(), StringComparison.InvariantCultureIgnoreCase) ?? false, "System.Collections.IEnumerable").Length > 0)
            canBeNullable = true;

        var column = create.WithColumn(columnName);

        TypeMapping[propType](column);

        if (propType == typeof(DateTime))
            create.CurrentColumn.Precision = DATE_TIME_PRECISION;

        if (canBeNullable)
            create.Nullable();
    }

    #endregion
}