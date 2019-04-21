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
            return type.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, argumentTypes?.ToArray(), null);
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

        public static Expression ForEach(Expression collection, ParameterExpression loopVar, Expression loopContent)
        {
            var elementType = loopVar.Type;
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);
            var enumeratorType = typeof(IEnumerator<>).MakeGenericType(elementType);

            var enumeratorVar = Expression.Variable(enumeratorType, "enumerator");
            var getEnumeratorCall = Expression.Call(collection, enumerableType.GetMethod("GetEnumerator"));
            var enumeratorAssign = Expression.Assign(enumeratorVar, getEnumeratorCall);

            // The MoveNext method's actually on IEnumerator, not IEnumerator<T>
            var moveNextCall = Expression.Call(enumeratorVar, typeof(IEnumerator).GetMethod("MoveNext"));

            var breakLabel = Expression.Label("LoopBreak");

            var loop = Expression.Block(new[] { enumeratorVar },
                enumeratorAssign,
                Expression.Loop(
                    Expression.IfThenElse(
                        Expression.Equal(moveNextCall, Expression.Constant(true)),
                        Expression.Block(new[] { loopVar },
                            Expression.Assign(loopVar, Expression.Property(enumeratorVar, "Current")),
                            loopContent
                        ),
                        Expression.Break(breakLabel)
                    ),
                breakLabel)
            );

            return loop;
        }
    }
}
