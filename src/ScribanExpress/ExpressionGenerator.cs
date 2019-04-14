using Scriban.Syntax;
using ScribanExpress.UnitTests.Globals;
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
        public ExpressionGenerator()
        {
        }

        public Expression<Func<T, Y, string>> Generate<T, Y>(ScriptBlockStatement scriptBlockStatement)
        {
            ParameterExpression TopLevelParameter = Expression.Parameter(typeof(T));
            ParameterExpression LibaryParameter = Expression.Parameter(typeof(Y));
            Expression currentExpression = Expression.Constant(String.Empty);
            foreach (var statement in scriptBlockStatement.Statements)
            {
                switch (statement)
                {
                    case ScriptRawStatement scriptRawStatement:
                        var constant = Expression.Constant(scriptRawStatement.ToString());
                        currentExpression = Expression.Add(currentExpression, constant, typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) }));
                        break;
                    case ScriptExpressionStatement scriptExpressionStatement:
                        var expressionBody = GetExpressionBody(scriptExpressionStatement.Expression, TopLevelParameter);
                        expressionBody = AddToString(expressionBody);
                        currentExpression = Expression.Add(currentExpression, expressionBody, typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) }));
                        break;
                }
            }

            return Expression.Lambda<Func<T, Y, string>>(currentExpression, TopLevelParameter, LibaryParameter);
        }



        public Expression GetExpressionBody(ScriptExpression scriptExpression, ParameterExpression topParameterExpression)
        {
            Expression currentExpression = null;
            switch (scriptExpression)
            {
                case ScriptVariableGlobal scriptVariableGlobal:
                    currentExpression = Expression.Property(topParameterExpression, scriptVariableGlobal.Name);
                    break;
                case ScriptMemberExpression scriptMemberExpression:
                    var memberTarget = GetExpressionBody(scriptMemberExpression.Target, topParameterExpression);
                    var member = scriptMemberExpression.Member.Name;
                    currentExpression = Expression.Property(memberTarget, member);
                    break;
                case ScriptPipeCall scriptPipeCall:
                    var fromExpression = GetExpressionBody(scriptPipeCall.From, topParameterExpression);
                    var to = scriptPipeCall.To as ScriptMemberExpression;

                    var targetObject = to.Target as ScriptVariableGlobal;

                    var targetInfo = GetTarget(targetObject);

                    if (targetInfo.ScopeType == VariableType.Static)
                    {
                        var memberVariable = to.Member as ScriptVariableGlobal;
                        var methodIfo = targetInfo.TargetType
                            .GetMethod(memberVariable.Name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase); // at this point we want to pass in again, 
                        var methodCall = Expression.Call(null, methodIfo, new Expression[] { fromExpression });
                        currentExpression = methodCall;
                    }
                    break;
                case ScriptFunctionCall scriptFunctionCall:
                    //last item is not a  property, but a function, so we want to skip it, or a least make a call to the funtion
                    var targetobject = scriptFunctionCall.Target as ScriptMemberExpression; // this might be null lots
                    var functionTarget = GetExpressionBody(targetobject.Target, topParameterExpression);

                    //currently we only support Literal argurments
                    var argList = scriptFunctionCall.Arguments.Select(arg => Expression.Constant((arg as ScriptLiteral).Value, (arg as ScriptLiteral).Value.GetType()));
                    var methodName = targetobject.Member.Name;
                    currentExpression = Expression.Call(functionTarget, typeof(int).GetMethod(methodName, argList.Select(x => x.Type).ToArray()), argList);
                    break;
            }

            return currentExpression;
        }


        public Expression AddToString(Expression input) => (input.Type != typeof(string)) ? Expression.Call(input, "ToString", null, null) : input;




        VariableType GetParameterScope(string paramter)
        {
            // we could just search ModelContext.Global
            // or use variablestack, push and pop 
            switch (paramter)
            {
                case "string":
                    return VariableType.Static;
                default:
                    return VariableType.Model;
            }
        }

        VariableInfo GetTarget(ScriptVariableGlobal scriptVariableGlobal)
        {
            var parameterScope = GetParameterScope(scriptVariableGlobal.Name);
            if (parameterScope == VariableType.Static)
                // bug: StringGlobals should not be referenced here 
                return new VariableInfo { ScopeType = VariableType.Static, TargetType = typeof(StringGlobals) };
            else
                throw new NotImplementedException();

        }

    }

    public enum VariableType
    {
        Model,
        Static
    }

    public class VariableInfo
    {
        public VariableType ScopeType { get; set; }
        public Type TargetType { get; set; }
    }
}
