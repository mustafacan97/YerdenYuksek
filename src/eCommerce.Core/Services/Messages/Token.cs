namespace eCommerce.Core.Services.Messages;

public sealed class Token
{
    #region Constructure and Destructure

    public Token(string key, object value) : this(key, value, false)
    {
    }

    public Token(string key, object value, bool neverHtmlEncoded)
    {
        Key = key;
        Value = value;
        NeverHtmlEncoded = neverHtmlEncoded;
    }

    #endregion

    #region Public Properties

    public string Key { get; }

    public object Value { get; }

    public bool NeverHtmlEncoded { get; }

    #endregion

    #region Public Methods

    public override string ToString()
    {
        return $"{Key}: {Value}";
    }

    #endregion
}
