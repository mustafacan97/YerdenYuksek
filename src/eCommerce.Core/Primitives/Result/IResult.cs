namespace eCommerce.Core.Primitives;

public interface IResult<T>
{
    #region Public Properties

    ResultStatus Status { get; }

    IEnumerable<Error> Errors { get; }

    Type? ValueType { get; }

    #endregion

    #region Public Methods

    T? GetValue();

    #endregion
}
