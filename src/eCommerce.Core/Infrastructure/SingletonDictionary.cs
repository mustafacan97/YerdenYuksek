namespace YerdenYuksek.Core.Infrastructure;

public class SingletonDictionary<TKey, TValue> : Singleton<IDictionary<TKey, TValue>>
{
    static SingletonDictionary()
    {
        Singleton<Dictionary<TKey, TValue>>.Instance = new Dictionary<TKey, TValue>();
    }

    public static new IDictionary<TKey, TValue> Instance => Singleton<Dictionary<TKey, TValue>>.Instance;
}