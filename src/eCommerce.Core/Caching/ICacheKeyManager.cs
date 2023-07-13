﻿namespace eCommerce.Core.Caching;

public interface ICacheKeyManager
{
    void AddKey(string key);

    void RemoveKey(string key);

    void Clear();

    IEnumerable<string> RemoveByPrefix(string prefix);

    IEnumerable<string> Keys { get; }
}