using ScribanExpress.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScribanExpress.Helpers
{
    public class ReflectionHelpers
    {
        public static PropertyInfo GetProperty(Type type, string memberName)
        {
            return type.GetProperty(memberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
        }

        public static MethodInfo GetMethod(Type type, string methodName, IEnumerable<Type> argumentTypes)
        {
            return type.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, argumentTypes.ToNullSafe().ToArray(), null);
        }
    }
}
