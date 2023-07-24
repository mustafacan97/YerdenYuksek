using LinqToDB;

namespace eCommerce.Infrastructure.Persistence;

public class TempSqlDataStorage<T> : TempTable<T>, ITempDataStorage<T> where T : class
{
    #region Ctor

    public TempSqlDataStorage(string storageName, IQueryable<T> query, IDataContext dataConnection)
        : base(dataConnection, storageName, query, tableOptions: TableOptions.NotSet | TableOptions.DropIfExists)
    {
        dataConnection.CloseAfterUse = true;
    }

    #endregion
}