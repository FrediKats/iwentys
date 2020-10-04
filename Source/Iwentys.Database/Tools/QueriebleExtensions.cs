using System;
using System.Linq;

namespace Iwentys.Database.Tools
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T, TFilter>(this IQueryable<T> query, TFilter? filter, Func<T, TFilter, bool> func) where TFilter : struct
        {
            return filter is null
                ? query
                : query.Where(q => func(q, filter.Value));
        }
    }
}