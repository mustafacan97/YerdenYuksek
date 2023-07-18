namespace eCommerce.Core.Primitives;

public interface IPagedInfo<T> : IList<T>
{
    #region Public Properties

    int PageIndex { get; }

    int PageSize { get; }

    int TotalCount { get; }

    int TotalPages { get; }

    bool HasPreviousPage { get; }

    bool HasNextPage { get; }

    #endregion
}
