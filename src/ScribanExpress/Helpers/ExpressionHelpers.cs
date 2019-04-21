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
        public static PropertyInfo GetProperty(Type type, string memberName)
        {
            return type.GetProperty(memberName, BindingFlags.IgnoreCase | BindingFlags.Instance  | BindingFlags.Public);
        }

        public static MethodInfo GetMethod(Type type, string methodName, IEnumerable<Type> argumentTypes)
        {
            return type.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, argumentTypes?.ToArray(), null);
        }

        public static MethodCallExpression CallMethod(MethodInfo methodInfo, Expression targetObject, IEnumerable<Expression> arguments)
        {
            if (methodInfo.IsStatic)
            {
                return Expression.Call(null, methodInfo, arguments);
            }
            else
            {
                return Expression.Call(targetObject, methodInfo, arguments);
            }
        }


    }
}
