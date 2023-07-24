using eCommerce.Core.Primitives;
using LinqToDB.Data;
using System.Linq.Expressions;

namespace eCommerce.Infrastructure.Persistence.DataProviders;

public interface ICustomDataProvider
{
    #region Public Properties

    string ConfigurationName { get; }

    int SupportedLengthOfBinaryHash { get; }

    bool BackupSupported { get; }

    #endregion

    #region Public Methods

    void CreateDatabase(string collation, int triesToConnect = 10);

    Task<ITempDataStorage<TItem>> CreateTempDataStorageAsync<TItem>(string storeKey, IQueryable<TItem> query) where TItem : class;

    Task InsertEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;

    void InsertEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;

    Task UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;

    void UpdateEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;

    Task UpdateEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;

    void UpdateEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;

    Task DeleteEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;

    void DeleteEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;

    Task BulkDeleteEntitiesAsync<TEntity>(IList<TEntity> entities) where TEntity : BaseEntity;

    void BulkDeleteEntities<TEntity>(IList<TEntity> entities) where TEntity : BaseEntity;

    Task<int> BulkDeleteEntitiesAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;

    int BulkDeleteEntities<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;

    Task BulkInsertEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;

    void BulkInsertEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;

    string CreateForeignKeyName(string foreignTable, string foreignColumn, string primaryTable, string primaryColumn);

    string GetIndexName(string targetTable, string targetColumn);

    IQueryable<TEntity> GetTable<TEntity>() where TEntity : BaseEntity;

    Task<int?> GetTableIdentAsync<TEntity>() where TEntity : BaseEntity;

    Task<bool> DatabaseExistsAsync();

    bool DatabaseExists();

    Task BackupDatabaseAsync(string fileName);

    Task RestoreDatabaseAsync(string backupFileName);

    Task ReIndexTablesAsync();

    Task SetTableIdentAsync<TEntity>(int ident) where TEntity : BaseEntity;

    Task<IDictionary<int, string>> GetFieldHashesAsync<TEntity>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, int>> keySelector,
        Expression<Func<TEntity, object>> fieldSelector) where TEntity : BaseEntity;

    Task<int> ExecuteNonQueryAsync(string sql, params DataParameter[] dataParameters);

    Task<IList<T>> QueryProcAsync<T>(string procedureName, params DataParameter[] parameters);

    Task<IList<T>> QueryAsync<T>(string sql, params DataParameter[] parameters);

    Task TruncateAsync<TEntity>(bool resetIdentity = false) where TEntity : BaseEntity;

    #endregion    
}
