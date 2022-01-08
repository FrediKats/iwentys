using System;
using System.Threading.Tasks;

namespace Iwentys.Common
{
    public static class Monads
    {
        public static TResult Maybe<TValue, TResult>(this TValue value, Func<TValue, TResult> just)
            where TValue : class
        {
            return value is null ? default : just(value);
        }

        public static async Task<TResult> To<TValue, TResult>(this Task<TValue> value, Func<TValue, TResult> convertor)
            where TValue : class
        {
            return convertor(await value);
        }

        public static TResult To<TValue, TResult>(this TValue value, Func<TValue, TResult> convertor)
            where TValue : class
        {
            return convertor(value);
        }
    }
}