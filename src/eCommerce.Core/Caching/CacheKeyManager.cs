using YerdenYuksek.Core.Infrastructure;

namespace YerdenYuksek.Core.Caching;

public class CacheKeyManager : ICacheKeyManager
{
    #region Fields

    protected readonly IConcurrentCollection<byte> _keys;

    #endregion

    #region Constructure and Destructure

    public CacheKeyManager(IConcurrentCollection<byte> keys)
    {
        _keys = keys;
    }

    #endregion

    #region Public Methods

    public void AddKey(string key)
    {
        _keys.Add(key, default);
    }

    public void RemoveKey(string key)
    {
        _keys.Remove(key);
    }

    public void Clear()
    {
        _keys.Clear();
    }

    public IEnumerable<string> RemoveByPrefix(string prefix)
    {
        if (!_keys.Prune(prefix, out var subtree) || subtree?.Keys == null)
            return Enumerable.Empty<string>();

        return subtree.Keys;
    }

    public IEnumerable<string> Keys => _keys.Keys;

    #endregion
}
