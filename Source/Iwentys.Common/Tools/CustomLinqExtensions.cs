using System;
using System.Collections.Generic;
using System.Linq;

namespace Iwentys.Common
{
    public static class CustomLinqExtensions
    {
        public static TResult[] SelectToArray<T, TResult>(this IEnumerable<T> source, Func<T, TResult> morphism)
        {
            return source.Select(morphism).ToArray();
        }

        public static List<TResult> SelectToList<T, TResult>(this IEnumerable<T> source, Func<T, TResult> morphism)
        {
            return source.Select(morphism).ToList();
        }

        public static HashSet<TResult> SelectToHashSet<T, TResult>(this IEnumerable<T> source, Func<T, TResult> morphism)
            where TResult : IComparable<TResult>
        {
            return source.Select(morphism).ToHashSet();
        }
    }
}