namespace YerdenYuksek.Core.Infrastructure;

public interface IConcurrentCollection<TValue>
{
    #region Public Properties

    IEnumerable<string> Keys { get; }

    #endregion

    #region Public Methods

    bool TryGetValue(string key, out TValue value);

    void Add(string key, TValue value);

    void Clear();

    IEnumerable<KeyValuePair<string, TValue>> Search(string prefix);

    void Remove(string key);

    bool Prune(string prefix, out IConcurrentCollection<TValue> subCollection);

#endregion
}