namespace YerdenYuksek.Core.Primitives;

public class Result
{
    #region Constructure and Destructure

    protected Result()
    {
    }

    protected Result(Error error)
    {        
       Errors.Add(error);        
    }

    protected Result(List<Error> errorList)
    {
        Errors.AddRange(errorList);
    }

    #endregion

    #region Public Properties

    public bool IsFailure => Errors.Any();

    public bool IsSuccess => !IsFailure;

    public List<Error> Errors { get; private set; } = new();

    #endregion    

    public static Result Success() => new();

    public static Result<TValue> Success<TValue>(TValue value) => new(value);

    public static Result Failure(Error error) => new(error);

    public static Result Failure(List<Error> errors) => new(errors);

    public static Result<TValue> Failure<TValue>(Error error) => new(default!, error);
}
