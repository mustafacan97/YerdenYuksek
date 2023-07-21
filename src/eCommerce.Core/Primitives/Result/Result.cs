namespace eCommerce.Core.Primitives;

public class Result
{
    #region Constructure and Destructure

    public Result() { }

    protected Result(ResultStatus status)
    {
        Status = status;
    }

    #endregion

    #region Public Properties

    public Guid CorrelationId { get; set; }

    public ResultStatus Status { get; set; } = ResultStatus.Ok;

    public IEnumerable<Error> Errors { get; set; } = new List<Error>();

    #endregion

    #region Public Methods

    public bool IsSuccess => Status == ResultStatus.Ok && !Errors.Any();

    public static Result Success() => new();

    public static Result Failure(params Error[] errors) => new(ResultStatus.Error) { Errors = errors };

    public static Result FailureWithCorrelationId(Guid correlationId, params Error[] errors) => new(ResultStatus.Error) { CorrelationId = correlationId, Errors = errors };

    public static Result Invalid(params Error[] errors) => new(ResultStatus.Invalid) { Errors = errors };

    public static Result NotFound() => new(ResultStatus.NotFound);

    public static Result NotFound(params Error[] errors) => new(ResultStatus.NotFound) { Errors = errors };

    public static Result Forbidden() => new(ResultStatus.Forbidden);

    public static Result Forbidden(params Error[] errors) => new(ResultStatus.NotFound) { Errors = errors };

    public static Result Unauthorized() => new(ResultStatus.Unauthorized);

    public static Result Conflict() => new(ResultStatus.Conflict);

    public static Result Conflict(params Error[] errors) => new(ResultStatus.Conflict) { Errors = errors };

    #endregion
}