using Microsoft.AspNetCore.Mvc.Rendering;
using eCommerce.Core.Primitives;

namespace eCommerce.Infrastructure.Concretes;

public static class Extensions
{
    public static SelectList ToSelectList<T>(this T objList, Func<BaseEntity, string> selector) where T : IEnumerable<BaseEntity>
    {
        return new SelectList(objList.Select(p => new { ID = p.Id, Name = selector(p) }), "ID", "Name");
    }

    public static IDictionary<TKey, IList<TValue>> ToGroupedDictionary<T, TKey, TValue>(
      this IEnumerable<T> xs,
      Func<T, TKey> keySelector,
      Func<T, TValue> valueSelector)
    {
        var result = new Dictionary<TKey, IList<TValue>>();

        foreach (var x in xs)
        {
            var key = keySelector(x);
            var value = valueSelector(x);

            if (result.TryGetValue(key, out var list))
                list.Add(value);
            else
                result[key] = new List<TValue> { value };
        }

        return result;
    }

    public static IDictionary<TKey, IList<T>> ToGroupedDictionary<T, TKey>(
      this IEnumerable<T> xs,
      Func<T, TKey> keySelector)
    {
        return xs.ToGroupedDictionary(keySelector, x => x);
    }
}
