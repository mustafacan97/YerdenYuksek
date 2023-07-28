using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Security;

public class Token : BaseEntity
{
    #region Constructure and Destructure

    private Token(string value, Guid customerId, TokenType type, DateTime expiredOnUtc)
    {
        Value = value;
        CustomerId = customerId;
        Status = TokenStatus.Active;
        Type = type;
        CreatedOnUtc = DateTime.UtcNow;
        ExpiredOnUtc = expiredOnUtc;
    }

    #endregion

    #region Public Properties

    public string Value { get; private set; }

    public Guid CustomerId { get; private set; }

    public TokenType Type { get; private set; }

    public TokenStatus Status { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime ExpiredOnUtc { get; private set; }

    #endregion

    #region Public Methods

    public static Token Create(string Value, Guid CustomerId, TokenType Type, DateTime ExpiredOnUtc) => new(Value, CustomerId, Type, ExpiredOnUtc);

    #endregion
}
