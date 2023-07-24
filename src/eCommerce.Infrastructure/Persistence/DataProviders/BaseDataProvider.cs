using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using eCommerce.Core.Primitives;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Tools;
using Microsoft.Extensions.Configuration;

namespace eCommerce.Infrastructure.Persistence.DataProviders;

public abstract class BaseDataProvider
{
    #region Fields

    private readonly IConfiguration _configuration;

    #endregion

    #region Constructure and Destructure

    protected BaseDataProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion    

    #region Properties

    protected abstract IDataProvider LinqToDbDataProvider { get; }

    public string ConfigurationName => LinqToDbDataProvider.Name;

    #endregion

    #region Public Methods

    public Task<ITempDataStorage<TItem>> CreateTempDataStorageAsync<TItem>(string storeKey, IQueryable<TItem> query) where TItem : class
    {
        return Task.FromResult<ITempDataStorage<TItem>>(new TempSqlDataStorage<TItem>(storeKey, query, CreateDataConnection()));
    }

    public async Task<IDictionary<int, string>> GetFieldHashesAsync<TEntity>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, int>> keySelector,
        Expression<Func<TEntity, object>> fieldSelector) where TEntity : BaseEntity
    {
        if (keySelector.Body is not MemberExpression keyMember || keyMember.Member is not PropertyInfo keyPropInfo)
        {
            throw new ArgumentException($"Expression '{keySelector}' refers to method or field, not a property.");
        }

        if (fieldSelector.Body is not MemberExpression member || member.Member is not PropertyInfo propInfo)
        {
            throw new ArgumentException($"Expression '{fieldSelector}' refers to a method or field, not a property.");
        }

        var hashes = GetTable<TEntity>()
            .Where(predicate)
            .Select(x => new
            {
                Id = Sql.Property<int>(x, keyPropInfo.Name),
                Hash = SqlSha2(Sql.Property<object>(x, propInfo.Name))
            });

        return await AsyncIQueryableExtensions.ToDictionaryAsync(hashes, p => p.Id, p => p.Hash);
    }

    public IQueryable<TEntity> GetTable<TEntity>() where TEntity : BaseEntity
    {
        return new DataContext(LinqToDbDataProvider, GetCurrentConnectionString()).GetTable<TEntity>();
    }

    public async Task InsertEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        await dataContext.InsertAsync(entity);
    }

    public void InsertEntity<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        dataContext.Insert(entity);
    }

    public async Task UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        await dataContext.UpdateAsync(entity);
    }

    public void UpdateEntity<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        dataContext.Update(entity);
    }

    public async Task UpdateEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
    {
        //we don't use the Merge API on this level, because this API not support all databases.
        //you may see all supported databases by the following link: https://linq2db.github.io/articles/sql/merge/Merge-API.html#supported-databases
        foreach (var entity in entities)
        {
            await UpdateEntityAsync(entity);
        }
    }

    public void UpdateEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
    {
        //we don't use the Merge API on this level, because this API not support all databases.
        //you may see all supported databases by the following link: https://linq2db.github.io/articles/sql/merge/Merge-API.html#supported-databases
        foreach (var entity in entities)
        {
            UpdateEntity(entity);
        }
    }

    public async Task DeleteEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        await dataContext.DeleteAsync(entity);
    }

    public void DeleteEntity<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        dataContext.Delete(entity);
    }

    public async Task BulkDeleteEntitiesAsync<TEntity>(IList<TEntity> entities) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        await dataContext.GetTable<TEntity>()
            .Where(e => e.Id.In(entities.Select(x => x.Id)))
            .DeleteAsync();
    }

    public void BulkDeleteEntities<TEntity>(IList<TEntity> entities) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        dataContext.GetTable<TEntity>()
            .Where(e => e.Id.In(entities.Select(x => x.Id)))
            .Delete();
    }

    public async Task<int> BulkDeleteEntitiesAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        return await dataContext.GetTable<TEntity>()
            .Where(predicate)
            .DeleteAsync();
    }

    public int BulkDeleteEntities<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        return dataContext.GetTable<TEntity>()
            .Where(predicate)
            .Delete();
    }

    public async Task BulkInsertEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection(LinqToDbDataProvider);
        await dataContext.BulkCopyAsync(new BulkCopyOptions(), entities.RetrieveIdentity(dataContext));
    }

    public void BulkInsertEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection(LinqToDbDataProvider);
        dataContext.BulkCopy(new BulkCopyOptions(), entities.RetrieveIdentity(dataContext));
    }

    public async Task<int> ExecuteNonQueryAsync(string sql, params DataParameter[] dataParameters)
    {
        return await CreateDbCommand(sql, dataParameters).ExecuteAsync();
    }

    public Task<IList<T>> QueryProcAsync<T>(string procedureName, params DataParameter[] parameters)
    {
        var command = CreateDbCommand(procedureName, parameters);
        var rez = command.QueryProc<T>()?.ToList();
        return Task.FromResult<IList<T>>(rez ?? new List<T>());
    }

    public Task<IList<T>> QueryAsync<T>(string sql, params DataParameter[] parameters)
    {
        using var dataContext = CreateDataConnection();
        return Task.FromResult<IList<T>>(dataContext.Query<T>(sql, parameters)?.ToList() ?? new List<T>());
    }

    public virtual async Task TruncateAsync<TEntity>(bool resetIdentity = false) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection(LinqToDbDataProvider);
        await dataContext.GetTable<TEntity>().TruncateAsync(resetIdentity);
    }

    #endregion

    #region Methods

    protected abstract DbConnection GetInternalDbConnection(string connectionString);

    protected virtual DataConnection CreateDataConnection() => CreateDataConnection(LinqToDbDataProvider);

    protected string GetCurrentConnectionString() => _configuration.GetConnectionString("ConnectionString");

    protected CommandInfo CreateDbCommand(string sql, DataParameter[] dataParameters)
    {
        if (dataParameters is null)
        {
            throw new ArgumentNullException(nameof(dataParameters));
        }

        var dataConnection = CreateDataConnection(LinqToDbDataProvider);

        return new CommandInfo(dataConnection, sql, dataParameters);
    }

    protected DataConnection CreateDataConnection(IDataProvider dataProvider)
    {
        if (dataProvider is null)
        {
            throw new ArgumentNullException(nameof(dataProvider));
        }

        return new DataConnection(dataProvider, CreateDbConnection());
    }

    protected DbConnection CreateDbConnection(string? connectionString = null)
    {
        return GetInternalDbConnection(!string.IsNullOrEmpty(connectionString) ? connectionString : GetCurrentConnectionString());
    }

    [Sql.Expression("CONVERT(VARCHAR(128), HASHBYTES('SHA2_512', SUBSTRING({0}, 0, 8000)), 2)", ServerSideOnly = true, Configuration = ProviderName.SqlServer)]
    [Sql.Expression("SHA2({0}, 512)", ServerSideOnly = true, Configuration = ProviderName.MySql)]
    [Sql.Expression("encode(digest({0}, 'sha512'), 'hex')", ServerSideOnly = true, Configuration = ProviderName.PostgreSQL)]
    protected static string SqlSha2(object binaryData)
    {
        throw new InvalidOperationException("This function should be used only in database code");
    }

    #endregion    
}