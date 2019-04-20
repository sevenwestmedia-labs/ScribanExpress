using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ScribanExpress.Helpers
{
    public class ExpressionHelpers
    {


        public static bool PropertyExists(Type type, string propertyName)
        {
            return type.GetProperty(propertyName) != null;
        }

        public static bool MethodExists(Type type, string methodName, IEnumerable<Type> argTypes)
        {
            return type.GetMethod(methodName, (argTypes ?? Enumerable.Empty<Type>()).ToArray() ) != null;
        }

        public static MethodInfo GetMethod(Type type, string methodName, IEnumerable<Type> argumentTypes)
        {
            return type.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, argumentTypes?.ToArray(), null);
        }


    }
}
