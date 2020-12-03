using System;
using System.Linq;
using System.Linq.Expressions;

namespace Iwentys.Common.Tools
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T, TFilter>(this IQueryable<T> query, TFilter? filter, Expression<Func<T, bool>> func) where TFilter : struct
        {
            return filter is null
                ? query
                : query.Where(func);
        }
    }
}