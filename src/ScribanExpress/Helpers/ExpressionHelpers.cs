using ScribanExpress.Extensions;
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


        public static MethodCallExpression CallMethod(Expression instance, MethodInfo methodInfo,  IEnumerable<Expression> arguments)
        {
            if (methodInfo.IsStatic)
            {
                return Expression.Call(null, methodInfo, arguments);
            }
            else
            {
                return Expression.Call(instance, methodInfo, arguments);
            }
        }

        public static Expression CallMember(Expression instance, MemberInfo memberInfo, IEnumerable<Expression> arguments)
        {

            switch (memberInfo)
            {
                case PropertyInfo propertyInfo:
                    return Expression.Property(instance, propertyInfo);
                case MethodInfo methodInfo:
                    return  ExpressionHelpers.CallMethod(instance, methodInfo,  arguments);
                default:
                    throw new NotSupportedException(); 
            }

        }

        //http://blog.functionalfun.net/2009/10/getting-methodinfo-of-generic-method.html
        /// <summary>
        /// Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(Expression<Action> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        /// <summary>
        /// Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(LambdaExpression expression)
        {
            MethodCallExpression outermostExpression = expression.Body as MethodCallExpression;

            if (outermostExpression == null)
            {
                throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
            }

            return outermostExpression.Method;
        }

        // todo look into dispose version
        // https://stackoverflow.com/questions/27175558/foreach-loop-using-expression-trees
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
