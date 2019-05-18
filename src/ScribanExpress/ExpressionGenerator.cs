using Scriban.Syntax;
using ScribanExpress.Exceptions;
using ScribanExpress.Extensions;
using ScribanExpress.Helpers;
using ScribanExpress.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ScribanExpress
{
    public class ExpressionGenerator
    {
        private readonly MemberFinder memberFinder;
        private readonly TypeConverter typeConverter;

        public ExpressionGenerator()
        {
            memberFinder = new MemberFinder();
            typeConverter = new TypeConverter();
        }










        public Expression GetExpressionBody(ScriptExpression scriptExpression, ParameterFinder parameterFinder, List<Expression> arguments)
        {
            switch (scriptExpression)
            {
                case ScriptVariableGlobal scriptVariableGlobal:
                    var variable = parameterFinder.GetProperty(scriptVariableGlobal.Name);
                    if (variable == null)
                    {
                        throw new SpanException($"Variable Not Found: {scriptVariableGlobal.Name}", scriptVariableGlobal.Span);
                    }
                    return variable;

                case ScriptLiteral scriptLiteral:
                    return Expression.Constant(scriptLiteral.Value, scriptLiteral.Value.GetType());

                case ScriptUnaryExpression scriptUnaryExpression:
                    switch (scriptUnaryExpression.Operator)
                    {
                        case ScriptUnaryOperator.Not:
                            var right = GetExpressionBody(scriptUnaryExpression.Right, parameterFinder, arguments);
                            return Expression.Not(right);

                        default:
                            throw new SpanException($"Unknown ScriptUnaryOperator: {scriptUnaryExpression.Operator}", scriptUnaryExpression.Span);
                    }

                case ScriptMemberExpression scriptMemberExpression:
                    // it's impossible to tell if we have a member or a method, so we check for both
                    var memberTarget = GetExpressionBody(scriptMemberExpression.Target, parameterFinder, null);
                    var memberName = scriptMemberExpression.Member.Name;

                    // TODO: we should remove the need to calculate a method with Args here, should not need to pass down info
                    // still need the argument list as ScriptPipeCall still needs to pass the args in
                    var argumentTypeList = arguments.ToNullSafe().Select(e => e.Type);

                    var methodInfo = memberFinder.FindMember(memberTarget.Type, memberName, argumentTypeList);
                    if (methodInfo != null)
                    {
                        var convertedArgs = ConvertArgs(methodInfo as MethodInfo, arguments);
                        return ExpressionHelpers.CallMember(memberTarget, methodInfo, convertedArgs);
                    }
                    throw new SpanException($"Member Not Found: {memberName}", scriptMemberExpression.Span);

                case ScriptPipeCall scriptPipeCall:
                    // I'm not a huge fan of this because it requires pushing args down to sub nodes, could cause issues with multi funtions tree
                    var fromExpression = GetExpressionBody(scriptPipeCall.From, parameterFinder, null);
                    // prepare args (input type + ScriptFunctionCall args )
                    var pipelineArgs = new List<Expression> { fromExpression };

                    switch (scriptPipeCall.To)
                    {
                        case ScriptFunctionCall scriptFunctionCall:
                            pipelineArgs.AddRange(scriptFunctionCall.Arguments.Select(arg => GetExpressionBody(arg, parameterFinder, null)));
                            return GetExpressionBody(scriptFunctionCall.Target, parameterFinder, pipelineArgs);
                        case ScriptMemberExpression scriptMemberExpression:
                            return GetExpressionBody(scriptMemberExpression, parameterFinder, pipelineArgs);
                        case ScriptPipeCall toScriptPipeCall:
                            var nestedfromExpression = GetExpressionBody(toScriptPipeCall.From, parameterFinder, pipelineArgs);
                            return GetExpressionBody(toScriptPipeCall.To, parameterFinder, new List<Expression> { nestedfromExpression });
                        default:
                            throw new NotSupportedException("Pipeline Expression Not Supported");
                    }

                case ScriptFunctionCall scriptFunctionCall:
                    return CalculateScriptFunctionCall(scriptFunctionCall, parameterFinder, arguments);

                case ScriptNestedExpression scriptNestedExpression:
                    return GetExpressionBody(scriptNestedExpression.Expression, parameterFinder, arguments);

                case ScriptBinaryExpression scriptBinaryExpression:
                    var leftExpression = GetExpressionBody(scriptBinaryExpression.Left, parameterFinder, null);
                    var rightExpression = GetExpressionBody(scriptBinaryExpression.Right, parameterFinder, null);
                    
                    switch (scriptBinaryExpression.Operator)
                    {
                        case ScriptBinaryOperator.Add:
                            leftExpression = ConvertIfNeeded(leftExpression, rightExpression.Type);
                            rightExpression = ConvertIfNeeded(rightExpression, leftExpression.Type);
                            return Expression.Add(leftExpression, rightExpression);
                        case ScriptBinaryOperator.EmptyCoalescing:
                            return Expression.Coalesce(leftExpression, rightExpression);
                        case ScriptBinaryOperator.CompareEqual:
                            return Expression.Equal(leftExpression, rightExpression);
                        default:
                            throw new SpanException($"Unknown ScriptBinaryExpression Operator: {scriptBinaryExpression.Operator}", scriptBinaryExpression.Span);
                    }
                case ScriptIndexerExpression scriptIndexerExpression:
                    //https://stackoverflow.com/questions/31924907/accessing-elements-of-types-with-indexers-using-expression-trees
                    // TODO: consider wrapping an enumerable to an array
                    var arrayTarget = GetExpressionBody(scriptIndexerExpression.Target, parameterFinder, null);
                    var arrayIndex = GetExpressionBody(scriptIndexerExpression.Index, parameterFinder, null);
                    var indexed = Expression.Property(arrayTarget, "Item", arrayIndex);
                    return indexed;
                default:
                    throw new SpanException($"Unknown Expression Type: {scriptExpression?.GetType()}", scriptExpression.Span);
            }
        }

        public Expression CalculateScriptFunctionCall(ScriptFunctionCall scriptFunctionCall, ParameterFinder parameterFinder, List<Expression> arguments)
        {
            var args = scriptFunctionCall.Arguments.Select(arg => GetExpressionBody(arg, parameterFinder, null));
            //add first argument if it's being supplied (probably from ScriptPipeCall)
            if (arguments != null)
            {
                args = arguments.Union(args);
            }

            // we are attempting to pull the bottom target member up, so we know the method Name
            var toMember = scriptFunctionCall.Target as ScriptMemberExpression;
            if (toMember == null)
            {
                throw new NotSupportedException();
            }
            else
            {
                var argumentTypes = args.ToNullSafe().Select(e => e.Type);
                var targetType = GetExpressionBody(toMember.Target, parameterFinder, null);
                ScriptVariable functionNameScript = toMember.Member;
                var methodInfo = memberFinder.FindMember(targetType.Type, functionNameScript.Name, argumentTypes.ToArray());
                var convertedArgs = ConvertArgs(methodInfo as MethodInfo, args);
                return ExpressionHelpers.CallMember(targetType, methodInfo, convertedArgs);
            }
        }
        
        IEnumerable<Expression>  ConvertArgs(MethodInfo target, IEnumerable<Expression> expressions)
        {
            var methodParameters = target.GetParameters();

            var mappedArgs =  methodParameters.LeftZip(expressions.ToNullSafe(), (parameter, argument) => (parameter, argument));

            foreach (var (parameter, argumentExpression) in mappedArgs)
            {
                // no argument was supplied
                if (argumentExpression == null && parameter.IsOptional)
                {
                    yield return Expression.Constant(parameter.DefaultValue);
                }
                else
                {
                    yield return ConvertIfNeeded(argumentExpression, parameter.ParameterType);
                }
            }
        }
        public Expression ConvertIfNeeded(Expression from, Type toType)
        {
            if (typeConverter.CanConvert(from.Type, toType))
            {
                return Expression.Convert(from, toType);
            }
            else
            {
                return from;
            }
        }

    }
}
