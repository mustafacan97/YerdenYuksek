namespace YerdenYuksek.Core.Primitives;

public class Result<TValue> : Result
{
    #region Fields

    private readonly TValue _value;

    #endregion

    #region Constructure and Destructure

    protected internal Result(TValue value) => _value = value;

    protected internal Result(TValue value, Error error) : base(error) => _value = value;

    #endregion

    #region Public Properties

    public TValue Value => IsSuccess ?
        _value :
        throw new InvalidOperationException("The value of a failure result can not be accessed");

    #endregion

    public static implicit operator Result<TValue>(TValue value) => Success(value);    
}
