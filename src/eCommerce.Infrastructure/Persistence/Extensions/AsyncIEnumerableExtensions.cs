namespace System.Linq;

public static class AsyncIEnumerableExtensions
{
    public static IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, ValueTask<TResult>> predicate)
    {
        return source.ToAsyncEnumerable().SelectAwait(predicate);
    }

    public static Task<TSource> FirstOrDefaultAwaitAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate)
    {
        return source.ToAsyncEnumerable().FirstOrDefaultAwaitAsync(predicate).AsTask();
    }

    public static Task<bool> AllAwaitAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate)
    {
        return source.ToAsyncEnumerable().AllAwaitAsync(predicate).AsTask();
    }

    public static IAsyncEnumerable<TResult> SelectManyAwait<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<IList<TResult>>> predicate)
    {
        async ValueTask<IAsyncEnumerable<TResult>> getAsyncEnumerable(TSource items)
        {
            var rez = await predicate(items);
            return rez.ToAsyncEnumerable();
        }

        return source.ToAsyncEnumerable().SelectManyAwait(getAsyncEnumerable);
    }

    public static IAsyncEnumerable<TResult> SelectManyAwait<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<IEnumerable<TResult>>> predicate)
    {
        async ValueTask<IAsyncEnumerable<TResult>> getAsyncEnumerable(TSource items)
        {
            var rez = await predicate(items);
            return rez.ToAsyncEnumerable();
        }

        return source.ToAsyncEnumerable().SelectManyAwait(getAsyncEnumerable);
    }

    public static IAsyncEnumerable<TSource> WhereAwait<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate)
    {
        return source.ToAsyncEnumerable().WhereAwait(predicate);
    }

    public static Task<bool> AnyAwaitAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate)
    {
        return source.ToAsyncEnumerable().AnyAwaitAsync(predicate).AsTask();
    }

    public static Task<TSource> SingleOrDefaultAwaitAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate)
    {
        return source.ToAsyncEnumerable().SingleOrDefaultAwaitAsync(predicate).AsTask();
    }

    public static Task<List<TSource>> ToListAsync<TSource>(this IEnumerable<TSource> source)
    {
        return source.ToAsyncEnumerable().ToListAsync().AsTask();
    }

    public static IOrderedAsyncEnumerable<TSource> OrderByDescendingAwait<TSource, TKey>(
        this IEnumerable<TSource> source, 
        Func<TSource, ValueTask<TKey>> keySelector)
    {
        return source.ToAsyncEnumerable().OrderByDescendingAwait(keySelector);
    }

    public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupByAwait<TSource, TKey, TElement>(
        this IEnumerable<TSource> source, 
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TSource, ValueTask<TElement>> elementSelector)
    {
        return source.ToAsyncEnumerable().GroupByAwait(keySelector, elementSelector);
    }

    public static ValueTask<TAccumulate> AggregateAwaitAsync<TSource, TAccumulate>(
        this IEnumerable<TSource> source, 
        TAccumulate seed,
        Func<TAccumulate, TSource, ValueTask<TAccumulate>> accumulator)
    {
        return source.ToAsyncEnumerable().AggregateAwaitAsync(seed, accumulator);
    }

    public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAwaitAsync<TSource, TKey, TElement>(
        this IEnumerable<TSource> source, 
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TSource, ValueTask<TElement>> elementSelector) where TKey : notnull
    {
        return source.ToAsyncEnumerable().ToDictionaryAwaitAsync(keySelector, elementSelector);
    }

    public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupByAwait<TSource, TKey>(
        this IEnumerable<TSource> source, 
        Func<TSource, ValueTask<TKey>> keySelector)
    {
        return source.ToAsyncEnumerable().GroupByAwait(keySelector);
    }

    public static ValueTask<decimal> SumAwaitAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, ValueTask<decimal>> selector)
    {
        return source.ToAsyncEnumerable().SumAwaitAsync(selector);
    }
}