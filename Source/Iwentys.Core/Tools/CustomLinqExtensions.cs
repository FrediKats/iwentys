using System;
using System.Collections.Generic;
using System.Linq;

namespace Iwentys.Core.Tools
{
    public static class CustomLinqExtensions
    {
        public static TResult[] SelectToArray<T, TResult>(this IEnumerable<T> source, Func<T, TResult> morphism) => source.Select(morphism).ToArray();
        public static List<TResult> SelectToList<T, TResult>(this IEnumerable<T> source, Func<T, TResult> morphism) => source.Select(morphism).ToList();
    }
}