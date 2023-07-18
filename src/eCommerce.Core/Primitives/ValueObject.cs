namespace eCommerce.Core.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{    
    #region Public Operators

    public static bool operator ==(ValueObject one, ValueObject two) => EqualOperator(one, two);

    public static bool operator !=(ValueObject one, ValueObject two) => NotEqualOperator(one, two);

    #endregion

    #region Public Methods

    public bool Equals(ValueObject? other) => Equals(other);

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    #endregion

    #region Methods

    protected static bool EqualOperator(ValueObject left, ValueObject right)
    {
        // Biri null, diğeri null değilse bu if bloğuna girer
        if (left is null ^ right is null)
        {
            return false;
        }

        if (left is null)
        {
            return true;
        }

        return ReferenceEquals(left, right) || left.Equals(right);
    }

    protected static bool NotEqualOperator(ValueObject left, ValueObject right) => !(EqualOperator(left, right));

    protected abstract IEnumerable<object> GetEqualityComponents();

    #endregion
}
