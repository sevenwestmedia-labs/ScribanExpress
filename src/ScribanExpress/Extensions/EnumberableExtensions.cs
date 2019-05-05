using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScribanExpress.Extensions
{
   public static class EnumberableExtensions
    {
        public static IEnumerable<T> ToNullSafe<T>(this IEnumerable<T> enumerable) => enumerable ?? Enumerable.Empty<T>();

        public static IEnumerable<(int index, T item)> GetIndexedEnumerable<T>(this IEnumerable<T> enumerable) {
            int i = 0;
            foreach (var item in enumerable)
            {
                yield return (i, item);
                i++;
            }
        }
    }
}
