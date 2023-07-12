using Microsoft.EntityFrameworkCore;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Web.Framework.Persistence.Extensions;

public static class AsyncIQueryableExtensions
{
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, bool getOnlyTotalCount = false)
    {
        if (source == null)
        {
            return new PagedInfo<T>(new List<T>(), pageIndex, pageSize);
        }
        
        pageSize = Math.Max(pageSize, 1);

        var count = await source.CountAsync();

        var data = new List<T>();

        if (!getOnlyTotalCount)
        {
            data.AddRange(await source.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync());
        }

        return new PagedInfo<T>(data, pageIndex, pageSize, count);
    }
}
