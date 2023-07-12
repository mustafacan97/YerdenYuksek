using eCommerce.Core.Primitives;

namespace eCommerce.Core.Primitives;

public class PagedResult<T> : Result<T>
{
    #region Constructure and Destructure

    public PagedResult(PagedInfo<T> pagedInfo, T? value) : base(value)
    {
        PagedInfo = pagedInfo;
    }

    #endregion

    #region Public Properties

    public PagedInfo<T> PagedInfo { get; }

    #endregion
}