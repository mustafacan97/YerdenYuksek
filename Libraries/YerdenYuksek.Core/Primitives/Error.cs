namespace YerdenYuksek.Core.Primitives;

public sealed class Error : IEquatable<Error>
{
    #region Constructure and Destructure

    private Error(string code, string description)
	{
		Code = code;
		Description = description;
	}

    private Error(string code, string description, ErrorType errorType)
    {
        Code = code;
        Description = description;
		ErrorType = errorType;
    }

    #endregion

    #region Public Properties

    public string Code { get; private set; }

	public string Description { get; private set; }

	public ErrorType? ErrorType { get; private set; }

    #endregion

    public static implicit operator string(Error error) => error?.Code ?? string.Empty;

	public static bool operator ==(Error a, Error b)
	{
		if (a is null && b is null) return true;

		if (a is null || b is null) return false;

		return a.Equals(b);
	}

	public static bool operator !=(Error a, Error b) => !(a == b);

	public bool Equals(Error? other)
	{
		if (other is null) return false;

		return 
			Code == other.Code && 
			Description == other.Description &&
			ErrorType == other.ErrorType;
	}

    public override bool Equals(object? obj)
    {
		if (obj is null) return false;

		if (obj is not Error error) return false;

		return Equals(error);
    }

	public override int GetHashCode() => HashCode.Combine(Code, Description, ErrorType);

    public static Error None => new(string.Empty, string.Empty);

    public static Error Failure(
        string code = "General.Failure",
        string description = "A failure has occurred.") => new(code, description, Primitives.ErrorType.Failure);

    public static Error Unexpected(
        string code = "General.Unexpected",
        string description = "An unexpected error has occurred.") => new(code, description, Primitives.ErrorType.UnExpected);

    public static Error Validation(
        string code = "General.Validation",
        string description = "A validation error has occurred.") => new(code, description, Primitives.ErrorType.Validation);

    public static Error Conflict(
        string code = "General.Conflict",
        string description = "A conflict error has occurred.") => new(code, description, Primitives.ErrorType.Conflict);

    public static Error NotFound(
        string code = "General.NotFound",
        string description = "A 'Not Found' error has occurred.") => new(code, description, Primitives.ErrorType.NotFound);

    public static Error Custom(string code, string description, ErrorType errorType) => new(code, description, errorType);
}
