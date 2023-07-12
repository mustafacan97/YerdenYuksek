namespace eCommerce.Core.Primitives;

public class Result : Result<Result>
{
    #region Constructure and Destructure

    public Result() : base() { }

    // Bu projede heryerde, diğer projelerde sadece bu sınıftan türeyen sınıflarda kullanılabilir!
    protected internal Result(ResultStatus status) : base(status) { }

    #endregion

    #region Public Methods

    public static Result Success() => new();

    public static Result SuccessWithMessage(string successMessage) => new() { SuccessMessage = successMessage };

    public static Result<T> Success<T>(T value) => new(value);

    public static Result<T> Success<T>(T value, string successMessage) => new(value, successMessage);

    public new static Result Failure(params Error[] errors) => new(ResultStatus.Error) { Errors = errors };

    public static Result FailureWithCorrelationId(Guid correlationId, params Error[] errors) => new(ResultStatus.Error) { CorrelationId = correlationId, Errors = errors };

    public new static Result Invalid(params Error[] errors) => new(ResultStatus.Invalid) { Errors = errors };

    public new static Result NotFound() => new(ResultStatus.NotFound);

    public new static Result NotFound(params Error[] errors) => new(ResultStatus.NotFound) { Errors = errors };

    public new static Result Forbidden() => new(ResultStatus.Forbidden);

    public new static Result Unauthorized() => new(ResultStatus.Unauthorized);

    public new static Result Conflict() => new(ResultStatus.Conflict);

    public new static Result Conflict(params Error[] errors) => new(ResultStatus.Conflict) { Errors = errors };

    #endregion
}