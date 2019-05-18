using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScribanExpress.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ToNullSafe<T>(this IEnumerable<T> enumerable) => enumerable ?? Enumerable.Empty<T>();

        public static IEnumerable<(int index, T item)> GetIndexedEnumerable<T>(this IEnumerable<T> enumerable)
        {
            int i = 0;
            foreach (var item in enumerable)
            {
                yield return (i, item);
                i++;
            }
        }

        public static IEnumerable<(TFirst Left, TSecond Right)> LeftZip<TFirst, TSecond>(this IEnumerable<TFirst> first,
                                                                                         IEnumerable<TSecond> second)
                                                                                         where TFirst : class where TSecond : class
        {
            return first.LeftZip(second, (Left, Right) => (Left, Right));
        }
            public static IEnumerable<TResult> LeftZip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first,
                                                                             IEnumerable<TSecond> second,
                                                                             Func<TFirst, TSecond, TResult> resultSelector)
                                                                             where TFirst : class where TSecond : class
        {
            var secondEnumerator = second.GetEnumerator();
            foreach (var firstItem in first)
            {
                if (secondEnumerator.MoveNext())
                {
                    yield return resultSelector(firstItem, secondEnumerator.Current);
                }
                else
                {
                    yield return resultSelector(firstItem, default);
                }
            }
        }
    }
}
