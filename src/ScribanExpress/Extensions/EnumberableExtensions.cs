using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScribanExpress.Extensions
{
   public static  class EnumberableExtensions
    {
        public static IEnumerable<T> ToNullSafe<T>(this IEnumerable<T> enumerable) => enumerable ?? Enumerable.Empty<T>();
    }
}
