using ScribanExpress.UnitTests.Helpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace ScribanExpress.UnitTests
{
    public class StatementGeneratorTests
    {
        private readonly StatementGenerator statementGenerator;
        public StatementGeneratorTests()
        {
            statementGenerator = Factory.CreateStatementGenerator();
        }


        [Fact]
        public void RawScriptStatement()
        {
            var block = ScriptStatementHelper.CreateScriptBlockStatement();
            block.Statements.Add(ScriptStatementHelper.CreateScriptRawStatement("hello"));

            var action = statementGenerator.Generate<string, string>(block);

            var compiled = action.Compile();

            StringBuilder sb = new StringBuilder();
            compiled(sb, "", "");

            sb.ToString().ShouldBe("hello");
        }


        [Fact]
        public void MultiRawScript_ReturnsHelloWorld()
        {
            var block = ScriptStatementHelper.CreateScriptBlockStatement();
            block.Statements.Add(ScriptStatementHelper.CreateScriptRawStatement("hello"));
            block.Statements.Add(ScriptStatementHelper.CreateScriptRawStatement("world"));
                        
            var lambda = statementGenerator.Generate<string, object>(block);
            var result = ExecuteStatments(lambda, null, null);

            result.ShouldBe("helloworld");
        }

        [Fact]
        public void InvalidStatement_ReturnsValidStatementsOnly()
        {
            var block = ScriptStatementHelper.CreateScriptBlockStatement();
            block.Statements.Add(ScriptStatementHelper.CreateScriptRawStatement("hello"));

            var invalidStatement = ScriptStatementHelper.CreateScriptExpressionStatement(
                ScriptStatementHelper.CreateScriptVariableGlobal("doesnotexist")
                );
            block.Statements.Add(invalidStatement);

            block.Statements.Add(ScriptStatementHelper.CreateScriptRawStatement("world"));

            var lambda = statementGenerator.Generate<string, object>(block);
            var result = ExecuteStatments(lambda, null, null);

            result.ShouldBe("helloworld");
        }


        private string ExecuteStatments<T,Y>(Expression<Action<StringBuilder,T,Y>> lambda, T input, Y library)
        {
            var functor = lambda.Compile();

            var sb = new StringBuilder();
            functor(sb, input, library);

            return sb.ToString();
        }

    }
}
