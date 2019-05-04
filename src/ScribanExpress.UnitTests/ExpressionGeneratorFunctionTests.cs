using Microsoft.Extensions.Logging.Abstractions;
using Scriban;
using Scriban.Syntax;
using ScribanExpress.Functions;
using ScribanExpress.UnitTests.Helpers;
using ScribanExpress.UnitTests.Models;
using Shouldly;
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
                LastName = "Bob",
                Age = 23,
                Company = new Company { Title = "compname" },
                Birthday = DateTime.Parse("2019-6-22")
            };
        }

        [Theory]
        [InlineData(@"{{ person.FirstName | Test.Deep.Append ""abc""}}", "Billyabc", "Deep Pipeline with params")]
        [InlineData(@"{{ person.FirstName | Test.Repeat }}", "BillyBilly", "standard pipline")]
        [InlineData(@"{{ person.FirstName | Test.Swap ""abc"" }}", "abcBilly", "pipline multi args")]
        [InlineData(@"{{ ""Mr"" | Test.PrefixPerson  person }}", "Mr Bob", "pipline with rich parameter")]
        [InlineData(@"{{ person | Test.GetPersonName | Test.Swap ""abc"" }}", "abcBilly Bob", "multi pipline")]
        [InlineData(@"{{   ""abc"" | Test.StaticHello }}", "HelloabcStatic", "static method pipline")]
        public void Pipeline_Tests(string templateText, string resultText, string reason)
        {
            var presonwrapper = new { person };

            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(presonwrapper, template.Page.Body);

            var functor = result.Compile();

            var sb = new StringBuilder();
            functor(sb, presonwrapper, new RootLibary());

            sb.ToString().ShouldBe(resultText, reason);
        }

        [Theory]
        [InlineData(@"{{ Test.ReturnHello  }}", "Hello", "function with no args")]
        [InlineData(@"{{ Test.Repeat  ""abc"" }}", "abcabc", "standard literal func")]
        [InlineData(@"{{ Test.GetPersonName person }}", "Billy Bob", "rich argument")]
        [InlineData(@"{{ Test.Repeat  Test.ReturnHello  }}", "HelloHello", "function to function")]
        [InlineData(@"{{ person.Company.GetCompanyName  true  }}", "COMPNAME", "function to function")]
        [InlineData(@"{{ Test.StaticHello  ""abc"" }}", "HelloabcStatic", "static method")]
        [InlineData(@"{{ (person.Birthday | date.AddDays 3).tostring ""dd"" }}", "25", "conversion")]
        [InlineData(@"{{ test.GetStaticDate.tostring ""yyyy"" }}", "2019", "end function arguments")]
        public void Function_Tests(string templateText, string resultText, string reason)
        {
            var presonwrapper = new { person };

            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(presonwrapper, template.Page.Body);

            var functor = result.Compile();
            var sb = new StringBuilder();

            functor(sb, presonwrapper, new RootLibary());

            sb.ToString().ShouldBe(resultText, reason);
        }


        //we need to error better for unknow method
        // test with   var templateText = @"{{ person.FirstName | Test.Deep.Appender ""abc""}}";


        public Expression<Action<StringBuilder,T, RootLibary>> AnonGenerate<T>(T value, ScriptBlockStatement scriptBlockStatement)
        {
            return Factory.CreateStatementGenerator().Generate<T, RootLibary>(new ExpressContext(), scriptBlockStatement);
        }
    }
}
