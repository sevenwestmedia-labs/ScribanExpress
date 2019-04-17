using Scriban.Syntax;
using ScribanExpress.Helpers;
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
            ParameterExpression LibraryParameter = Expression.Parameter(typeof(Y));
            ParameterFinder parameterFinder = new ParameterFinder();
            parameterFinder.AddType(LibraryParameter);
            parameterFinder.AddType(TopLevelParameter);

            Expression currentExpression = Expression.Constant(string.Empty);
            foreach (var statement in scriptBlockStatement.Statements)
            {
                switch (statement)
                {
                    case ScriptRawStatement scriptRawStatement:
                        var constant = Expression.Constant(scriptRawStatement.ToString());
                        currentExpression = Expression.Add(currentExpression, constant, typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) }));
                        break;
                    case ScriptExpressionStatement scriptExpressionStatement:
                        var expressionBody = GetExpressionBody(scriptExpressionStatement.Expression, parameterFinder, null);
                        expressionBody = AddToString(expressionBody);
                        currentExpression = Expression.Add(currentExpression, expressionBody, typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) }));
                        break;
                }
            }

            return Expression.Lambda<Func<T, Y, string>>(currentExpression, TopLevelParameter, LibraryParameter);
        }



        public Expression GetExpressionBody(ScriptExpression scriptExpression, ParameterFinder parameterFinder, List<Expression> arguments)
        {
            Expression currentExpression = null;
            switch (scriptExpression)
            {
                case ScriptVariableGlobal scriptVariableGlobal:
                    var parameter = parameterFinder.Find(scriptVariableGlobal.Name);
                    currentExpression = Expression.Property(parameter, scriptVariableGlobal.Name);
                    break;

                case ScriptLiteral scriptLiteral:
                    currentExpression = Expression.Constant(scriptLiteral.Value, scriptLiteral.Value.GetType());
                    break;

                case ScriptMemberExpression scriptMemberExpression:
                    // it's impossible to tell if we have a member or a method, so we check for both
                    var memberTarget = GetExpressionBody(scriptMemberExpression.Target, parameterFinder, null);
                    var memberName = scriptMemberExpression.Member.Name;

                    if (ExpressionHelpers.PropertyExists(memberTarget.Type, memberName))
                    {
                        currentExpression = Expression.Property(memberTarget, memberName);
                    }
                    var argumentTypeList = arguments?.Select(e => e.Type) ?? Enumerable.Empty<Type>();
                    if (ExpressionHelpers.MethodExists(memberTarget.Type, memberName, argumentTypeList))
                    {
                        var methodIfo = memberTarget.Type.GetMethod(memberName, argumentTypeList.ToArray());
                        //var methodIfo = memberTarget.Type.GetMethod(memberName, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase, argumentTypeList.ToArray()); // at this point we want to pass in again, 
                        var methodCall = Expression.Call(memberTarget, methodIfo, arguments);
                        currentExpression = methodCall;
                    }
                    break;

                case ScriptPipeCall scriptPipeCall:
                    // I'm not a huge fan of this because it requires pushing args down to sub nodes, could cause issues with multi funtions tree
                    var fromExpression = GetExpressionBody(scriptPipeCall.From, parameterFinder, null);
                    // prepare args (input type + ScriptFunctionCall args )
                    var pipelineArgs = new List<Expression> { fromExpression };

                    switch (scriptPipeCall.To)
                    {
                        case ScriptFunctionCall scriptFunctionCall:
                            pipelineArgs.AddRange(scriptFunctionCall.Arguments.Select(arg => GetExpressionBody(arg, parameterFinder, null)));
                            currentExpression = GetExpressionBody(scriptFunctionCall.Target, parameterFinder, pipelineArgs);
                            break;
                        case ScriptMemberExpression scriptMemberExpression:
                            currentExpression = GetExpressionBody(scriptMemberExpression, parameterFinder, pipelineArgs);
                            break;
                        case ScriptPipeCall toScriptPipeCall:
                            var nestedfromExpression = GetExpressionBody(toScriptPipeCall.From, parameterFinder, pipelineArgs);
                            currentExpression = GetExpressionBody(toScriptPipeCall.To, parameterFinder, new List<Expression> { nestedfromExpression });
                            break;

                            // todo break on default
                    }

                    // may or may not have argments so will need two options here
                    //var toFunction = scriptPipeCall.To as ScriptFunctionCall;




                    break;
                case ScriptFunctionCall scriptFunctionCall:
                    var args = scriptFunctionCall.Arguments.Select(arg => GetExpressionBody(arg, parameterFinder, null));
                    currentExpression = GetExpressionBody(scriptFunctionCall.Target, parameterFinder, args.ToList<Expression>());
                    
                    
                    //last item is not a  property, but a function, so we want to skip it, or a least make a call to the funtion
                    //var targetobject = scriptFunctionCall.Target as ScriptMemberExpression; // this might be null lots
                    //var functionTarget = GetExpressionBody(targetobject.Target, parameterFinder, null);

                    //todo: currently we only support Literal argurments
                 //   var argList = scriptFunctionCall.Arguments.Select(arg => Expression.Constant((arg as ScriptLiteral).Value, (arg as ScriptLiteral).Value.GetType()));
                //    var methodName = targetobject.Member.Name;
                //    currentExpression = Expression.Call(functionTarget, typeof(int).GetMethod(methodName, argList.Select(x => x.Type).ToArray()), argList);
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
