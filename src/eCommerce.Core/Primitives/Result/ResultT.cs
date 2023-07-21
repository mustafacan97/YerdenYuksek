namespace eCommerce.Core.Primitives;

public class Result<T> : Result
{
    #region Constructure and Destructure

    public Result() { }

    protected Result(ResultStatus status) : base(status) { }

    protected Result(T value) => Value = value;

    #endregion

    #region Public Properties

    public T? Value { get; set; }

    #endregion

    #region Public Methods

    public static implicit operator T?(Result<T> result) => result.Value;

    public static implicit operator Result<T>(T value) => new(value);

    public T? GetValue() => Value;

    public PagedResult<T> ToPagedResult(PagedInfo<T> pagedInfo)
    {
        var pagedResult = new PagedResult<T>(pagedInfo, Value)
        {
            Status = Status,
            CorrelationId = CorrelationId,
            Errors = Errors
        };

        return pagedResult;
    }

    public static Result<T> Success(T value) => new(value);

    public new static Result<T> Success() => new();

    public new static Result<T> Failure(params Error[] errors) => new(ResultStatus.Error) { Errors = errors };

    public new static Result<T> Invalid(params Error[] errors) => new(ResultStatus.Invalid) { Errors = errors };

    public new static Result<T> NotFound() => new(ResultStatus.NotFound);

    public new static Result<T> NotFound(params Error[] errors) => new(ResultStatus.NotFound) { Errors = errors };

    public new static Result<T> Forbidden() => new(ResultStatus.Forbidden);

    public new static Result<T> Forbidden(params Error[] errors) => new(ResultStatus.Forbidden) { Errors = errors };

    public new static Result<T> Unauthorized() => new(ResultStatus.Unauthorized);

    public new static Result<T> Conflict() => new(ResultStatus.Conflict);

    public new static Result<T> Conflict(params Error[] errors) => new(ResultStatus.Conflict) { Errors = errors };

    #endregion
}
