using Scriban;
using Scriban.Syntax;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace ScribanExpress.UnitTests
{
    public class IfElseTests
    {
        [Fact]
        public void If()
        {
            var itemWrapper = new { item = new { someValue=true } };

            var templateText = @"{{ if item.someValue }}value is true{{ end }}";
            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(itemWrapper, template.Page.Body);

            var functor = result.Compile();

            var sb = new StringBuilder();
            functor(sb, itemWrapper, null);

            sb.ToString().ShouldBe("value is true");
        }


        public Expression<Action<StringBuilder, T, object>> AnonGenerate<T>(T value, ScriptBlockStatement scriptBlockStatement)
        {
            return new ExpressionGenerator().Generate<T, object>(scriptBlockStatement);
        }
    }
}
