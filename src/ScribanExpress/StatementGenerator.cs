using Microsoft.Extensions.Logging;
using Scriban.Syntax;
using ScribanExpress.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ScribanExpress
{
    public class StatementGenerator
    {
        private readonly ExpressionGenerator expressionGenerator;
        private readonly ILogger<StatementGenerator> logger;

        public StatementGenerator(ILogger<StatementGenerator> logger)
        {
            expressionGenerator = new ExpressionGenerator();
            this.logger = logger;
        }

        public Expression<Action<StringBuilder, T, Y>> Generate<T, Y>(ScriptBlockStatement scriptBlockStatement)
        {
            ParameterExpression StringBuilderParmeter = Expression.Parameter(typeof(StringBuilder));
            ParameterExpression InputParameter = Expression.Parameter(typeof(T));
            ParameterExpression LibraryParameter = Expression.Parameter(typeof(Y));

            ParameterFinder parameterFinder = new ParameterFinder();
            parameterFinder.AddType(LibraryParameter);
            parameterFinder.AddType(InputParameter);

            var blockExpression = GetStatementExpression(StringBuilderParmeter, scriptBlockStatement, parameterFinder);

            return Expression.Lambda<Action<StringBuilder, T, Y>>(blockExpression, StringBuilderParmeter, InputParameter, LibraryParameter);
        }

        public Expression GetStatementExpression(ParameterExpression stringBuilderParameter, ScriptStatement scriptStatement, ParameterFinder parameterFinder)
        {
            var appendMethodInfo = typeof(StringBuilder).GetMethod("Append", new[] { typeof(string) });

            switch (scriptStatement)
            {
                case ScriptRawStatement scriptRawStatement:
                    var constant = Expression.Constant(scriptRawStatement.ToString());
                    var methodCall = Expression.Call(stringBuilderParameter, appendMethodInfo, constant);
                    return methodCall;

                case ScriptExpressionStatement scriptExpressionStatement:
                    var expressionBody = expressionGenerator.GetExpressionBody(scriptExpressionStatement.Expression, parameterFinder, null);
                    expressionBody = AddToString(expressionBody);
                    var scriptmethodCall = Expression.Call(stringBuilderParameter, appendMethodInfo, expressionBody);
                    return scriptmethodCall;

                case ScriptIfStatement scriptIfStatement:
                    var predicateExpression = expressionGenerator.GetExpressionBody(scriptIfStatement.Condition, parameterFinder, null);
                    var trueStatementBlock = GetStatementExpression(stringBuilderParameter, scriptIfStatement.Then, parameterFinder);

                    if (scriptIfStatement.Else != null)
                    {
                        var elseStatment = GetStatementExpression(stringBuilderParameter, scriptIfStatement.Else, parameterFinder);
                        ConditionalExpression ifThenElseExpr = Expression.IfThenElse(predicateExpression, trueStatementBlock, elseStatment);
                        return ifThenElseExpr;
                    }
                    else
                    {
                        ConditionalExpression ifThenExpr = Expression.IfThen(predicateExpression, trueStatementBlock);
                        return ifThenExpr;
                    }

                case ScriptElseStatement scriptElseStatement:
                    var elseStatmentExpression = GetStatementExpression(stringBuilderParameter, scriptElseStatement.Body, parameterFinder);
                    return elseStatmentExpression;

                case ScriptBlockStatement scriptBlockStatement:
                    List<Expression> statements = new List<Expression>();
                    foreach (var statement in scriptBlockStatement.Statements)
                    {
                        try
                        {
                            statements.Add(GetStatementExpression(stringBuilderParameter, statement, parameterFinder));
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Statement Failed to build: {statement}", statement);
                        }
                    }
                    var blockExpression = Expression.Block(statements);
                    return blockExpression;

                case ScriptForStatement scriptForStatement:
                    // foreach(item in items)
                    var itemsExpression = expressionGenerator.GetExpressionBody(scriptForStatement.Iterator, parameterFinder, null);
                    var itemVariableName = (scriptForStatement.Variable as ScriptVariableGlobal).Name;
                    var itemType = itemsExpression.Type.GenericTypeArguments[0];
                    ParameterExpression itemVariable = Expression.Parameter(itemType, itemVariableName);

                    using (var scopedParameterFinder = parameterFinder.CreateScope())
                    {
                        scopedParameterFinder.AddLocalVariable(itemVariable);
                        var body = GetStatementExpression(stringBuilderParameter, scriptForStatement.Body, scopedParameterFinder);
                        var foreachExpression = ExpressionHelpers.ForEach(itemsExpression, itemVariable, body);
                        return foreachExpression;
                    }

                default:
                    throw new NotImplementedException("Unknown ScriptStatement");
            }
        }

        public Expression AddToString(Expression input) => (input.Type != typeof(string)) ? Expression.Call(input, "ToString", null, null) : input;
    }
}
