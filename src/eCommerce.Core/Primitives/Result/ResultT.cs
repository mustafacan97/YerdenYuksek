namespace eCommerce.Core.Primitives;

public class Result<T> : IResult<T>
{
    #region Constructure and Destructure

    // Bu sınıfta veya bu sınıftan türeyen sınıflarda kullanılabilir!
    protected Result() { }

    // Bu sınıfta veya bu sınıftan türeyen sınıflarda kullanılabilir!
    protected Result(ResultStatus status)
    {
        Status = status;
    }    

    // Bu projede heryerde, diğer projelerde sadece bu sınıftan türeyen sınıflarda kullanılabilir!
    protected internal Result(T value, string successMessage) : this(value)
    {
        SuccessMessage = successMessage;
    }

    public Result(T? value)
    {
        Value = value;

        if (Value is not null)
        {
            ValueType = Value.GetType();
        }
    }

    #endregion

    #region Public Properties

    public T? Value { get; }

    public Type? ValueType { get; private set; }

    public ResultStatus Status { get; protected set; } = ResultStatus.Ok;    

    public string SuccessMessage { get; protected set; } = string.Empty;

    public Guid CorrelationId { get; protected set; }

    public IEnumerable<Error> Errors { get; protected set; } = new List<Error>();

    #endregion

    #region Public Methods

    public static implicit operator T?(Result<T> result) => result.Value;

    public static implicit operator Result<T>(T value) => new(value);

    public static implicit operator Result<T>(Result result) => new(default(T))
    {
        Status = result.Status,
        Errors = result.Errors,
        SuccessMessage = result.SuccessMessage,
        CorrelationId = result.CorrelationId
    };    

    public bool IsSuccess => Status == ResultStatus.Ok && !Errors.Any();

    public void ClearValueType() => ValueType = null;

    public T? GetValue() => Value;

    public PagedResult<T> ToPagedResult(PagedInfo<T> pagedInfo)
    {
        var pagedResult = new PagedResult<T>(pagedInfo, Value)
        {
            Status = Status,
            SuccessMessage = SuccessMessage,
            CorrelationId = CorrelationId,
            Errors = Errors
        };

        return pagedResult;
    }

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Success(T value, string successMessage) => new(value, successMessage);

    public static Result<T> Failure(params Error[] errors) => new(ResultStatus.Error) { Errors = errors };

    public static Result<T> Invalid(params Error[] errors) => new(ResultStatus.Invalid) { Errors = errors };

    public static Result<T> NotFound() => new(ResultStatus.NotFound);

    public static Result<T> NotFound(params Error[] errors) => new(ResultStatus.NotFound) { Errors = errors };

    public static Result<T> Forbidden() => new(ResultStatus.Forbidden);

    public static Result<T> Forbidden(params Error[] errors) => new(ResultStatus.Forbidden) { Errors = errors };

    public static Result<T> Unauthorized() => new (ResultStatus.Unauthorized);

    public static Result<T> Conflict() => new(ResultStatus.Conflict);

    public static Result<T> Conflict(params Error[] errors) => new(ResultStatus.Conflict) { Errors = errors };

    #endregion
}
