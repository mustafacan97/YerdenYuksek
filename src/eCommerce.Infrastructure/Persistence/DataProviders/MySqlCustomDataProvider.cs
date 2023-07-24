using System.Data;
using System.Data.Common;
using System.Text;
using eCommerce.Core.Primitives;
using eCommerce.Core.Shared;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.MySql;
using LinqToDB.SqlQuery;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace eCommerce.Infrastructure.Persistence.DataProviders;

public class MySqlCustomDataProvider : BaseDataProvider, ICustomDataProvider
{
    #region Static Fields and Constants

    //it's quite fast hash (to cheaply distinguish between objects)
    private const string HASH_ALGORITHM = "SHA1";

    #endregion

    #region Constructure and Destructure

    public MySqlCustomDataProvider(IConfiguration configuration) : base(configuration)
    {
    }

    #endregion

    #region Properties

    protected override IDataProvider LinqToDbDataProvider => MySqlTools.GetDataProvider(ProviderName.MySqlConnector);

    public int SupportedLengthOfBinaryHash { get; }

    public bool BackupSupported => false;

    #endregion

    #region Public Methods

    public void CreateDatabase(string collation, int triesToConnect = 10)
    {
        if (DatabaseExists())
        {
            return;
        }

        var builder = GetConnectionStringBuilder();
        var databaseName = builder.Database;
        builder.Database = null;

        using (var connection = GetInternalDbConnection(builder.ConnectionString))
        {
            var query = $"CREATE DATABASE IF NOT EXISTS {databaseName}";
            if (!string.IsNullOrWhiteSpace(collation))
            {
                query = $"{query} COLLATE {collation}";
            }

            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Connection.Open();
            command.ExecuteNonQuery();
        }

        if (triesToConnect <= 0)
        {
            return;
        }

        for (var i = 0; i <= triesToConnect; i++)
        {
            if (i == triesToConnect)
            {
                throw new Exception("Unable to connect to the new database. Please try one more time");
            }

            if (!DatabaseExists())
            {
                Thread.Sleep(1000);
            }
            else
            {
                break;
            }
        }
    }

    public async Task<bool> DatabaseExistsAsync()
    {
        try
        {
            await using var connection = GetInternalDbConnection(GetCurrentConnectionString());
            await connection.OpenAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool DatabaseExists()
    {
        try
        {
            using var connection = CreateDbConnection();
            connection.Open();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<int?> GetTableIdentAsync<TEntity>() where TEntity : BaseEntity
    {
        using var currentConnection = CreateDataConnection();
        var tableName = typeof(TEntity).Name;
        var databaseName = currentConnection.Connection.Database;

        //we're using the DbConnection object until linq2db solve this issue https://github.com/linq2db/linq2db/issues/1987
        //with DataContext we could be used KeepConnectionAlive option
        await using var dbConnection = GetInternalDbConnection(GetCurrentConnectionString());

        dbConnection.StateChange += (sender, e) =>
        {
            try
            {
                if (e.CurrentState != ConnectionState.Open)
                    return;

                var connection = (IDbConnection)sender;
                using var internalCommand = connection.CreateCommand();
                internalCommand.Connection = connection;
                internalCommand.CommandText = "SET @@SESSION.information_schema_stats_expiry = 0;";
                internalCommand.ExecuteNonQuery();
            }
            //ignoring for older than 8.0 versions MySQL (#1193 Unknown system variable)
            catch (MySqlException ex) when (ex.Number == 1193)
            {
                //ignore
            }
        };

        await using var command = dbConnection.CreateCommand();
        command.Connection = dbConnection;
        command.CommandText = $"SELECT AUTO_INCREMENT FROM information_schema.TABLES WHERE TABLE_SCHEMA = '{databaseName}' AND TABLE_NAME = '{tableName}'";
        await dbConnection.OpenAsync();

        return Convert.ToInt32((await command.ExecuteScalarAsync()) ?? 1);
    }

    public async Task SetTableIdentAsync<TEntity>(int ident) where TEntity : BaseEntity
    {
        var currentIdent = await GetTableIdentAsync<TEntity>();
        if (!currentIdent.HasValue || ident <= currentIdent.Value)
        {
            return;
        }

        using var currentConnection = CreateDataConnection();
        var tableName = typeof(TEntity).Name;

        await currentConnection.ExecuteAsync($"ALTER TABLE `{tableName}` AUTO_INCREMENT = {ident};");
    }

    public Task BackupDatabaseAsync(string fileName)
    {
        throw new DataException("This database provider does not support backup");
    }

    public Task RestoreDatabaseAsync(string backupFileName)
    {
        throw new DataException("This database provider does not support backup");
    }

    public async Task ReIndexTablesAsync()
    {
        using var currentConnection = CreateDataConnection();
        var tables = currentConnection.Query<string>($"SHOW TABLES FROM `{currentConnection.Connection.Database}`").ToList();

        if (tables.Count > 0)
        {
            await currentConnection.ExecuteAsync($"OPTIMIZE TABLE `{string.Join("`, `", tables)}`");
        }
    }

    public string CreateForeignKeyName(string foreignTable, string foreignColumn, string primaryTable, string primaryColumn)
    {
        //mySql support only 64 chars for constraint name
        //that is why we use hash function for create unique name
        //see details on this topic: https://dev.mysql.com/doc/refman/8.0/en/identifier-length.html
        return "FK_" + EncryptionHelper.CreateHash(Encoding.UTF8.GetBytes($"{foreignTable}_{foreignColumn}_{primaryTable}_{primaryColumn}"), HASH_ALGORITHM);
    }

    public virtual string GetIndexName(string targetTable, string targetColumn)
    {
        return "IX_" + EncryptionHelper.CreateHash(Encoding.UTF8.GetBytes($"{targetTable}_{targetColumn}"), HASH_ALGORITHM);
    }

    #endregion

    #region Methods

    protected override DataConnection CreateDataConnection()
    {
        var dataContext = CreateDataConnection(LinqToDbDataProvider);
        dataContext.MappingSchema.SetDataType(typeof(Guid), new SqlDataType(DataType.NChar, typeof(Guid), 36));
        dataContext.MappingSchema.SetConvertExpression<string, Guid>(strGuid => new Guid(strGuid));

        return dataContext;
    }

    protected MySqlConnectionStringBuilder GetConnectionStringBuilder() => new MySqlConnectionStringBuilder(GetCurrentConnectionString());

    protected override DbConnection GetInternalDbConnection(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        return new MySqlConnection(connectionString);
    }

    #endregion
}
