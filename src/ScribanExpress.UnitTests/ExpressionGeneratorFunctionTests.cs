using Scriban;
using Scriban.Syntax;
using ScribanExpress.Functions;
using ScribanExpress.UnitTests.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace ScribanExpress.UnitTests
{
   public class ExpressionGeneratorFunctionTests
    {
        Person person;
        public ExpressionGeneratorFunctionTests()
        {
            person = new Person()
            {
                FirstName = "Billy",
                Age = 23,
                Company = new Company { Title = "compname" }
            };
        }
        [Theory]
        [InlineData(@"{{ person.FirstName | Test.Deep.Append ""abc""}}", "Billyabc", "Function with params")]
        [InlineData(@"{{ person.FirstName | Test.Repeat }}", "BillyBilly", "standard pipline")]
        [InlineData(@"{{ Test.Repeat  ""abc"" }}", "abcabc", "standard literal func")]
        public void NamedStringProperty(string templateText, string resultText, string reason)
        {
            var presonwrapper = new { person };

            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(presonwrapper, template.Page.Body);

            var functor = result.Compile();

            var stringResult = functor(presonwrapper, new RootLibary());

            Assert.Equal(resultText, stringResult);
        }

        public Expression<Func<T, RootLibary, string>> AnonGenerate<T>(T value, ScriptBlockStatement scriptBlockStatement)
        {
            return new ExpressionGenerator().Generate<T, RootLibary>(scriptBlockStatement);
        }
    }
}
