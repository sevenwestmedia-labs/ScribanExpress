using Scriban.Syntax;
using ScribanExpress.Extensions;
using ScribanExpress.Helpers;
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

        public ExpressionGenerator()
        {
            memberFinder = new MemberFinder();
        }










        public Expression GetExpressionBody(ScriptExpression scriptExpression, ParameterFinder parameterFinder, List<Expression> arguments)
        {
            switch (scriptExpression)
            {
                case ScriptVariableGlobal scriptVariableGlobal:
                    var variable = parameterFinder.GetProperty(scriptVariableGlobal.Name);
                    if (variable == null)
                    {
                        throw new KeyNotFoundException("Variable Not Found");
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
                            throw new NotImplementedException("Unknown ScriptUnaryOperator");
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
                       return ExpressionHelpers.CallMember(memberTarget, methodInfo, arguments);
                    }
                    throw new KeyNotFoundException("Member Not Found");

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
                            return Expression.Add(leftExpression, rightExpression);
                        case ScriptBinaryOperator.EmptyCoalescing:
                            return Expression.Coalesce(leftExpression, rightExpression);
                        default:
                            throw new NotImplementedException("Unknown ScriptBinaryExpression Operator");
                    }
                default:
                    throw new NotImplementedException($"Unknown Expression Type");
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
                var targetType = GetExpressionBody(toMember.Target, parameterFinder, args.ToList());
                ScriptVariable functionNameScript = toMember.Member;
                var methodInfo = memberFinder.FindMember(targetType.Type, functionNameScript.Name, argumentTypes.ToArray());
                return ExpressionHelpers.CallMember(targetType, methodInfo, args);
            }
        }
    }
}
