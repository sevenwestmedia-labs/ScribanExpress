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
    public class ExpressTemplateManagerTests
    {
        private readonly Person person;

        public ExpressTemplateManagerTests()
        {
            person = new Person()
            {
                FirstName = "Billy",
                LastName = "Bob",
                Age = 23,
                Company = new Company { Title = "compname" },
                Title = TitleType.Dr
            };

        }

        [Theory]
        [InlineData(@"{{ person.Title }}", "Dr", "enum")]
        [InlineData(@"{{ person.DontSet ?? ""set"" }}", "set", "Coalesce needed")]
        [InlineData(@"{{ person.FirstName ?? ""set"" }}", "Billy", "Coalesce unneeded")]
        [InlineData(@"{{if person.FirstName == ""Billy"" }}yes{{ end }}", "yes", " == ")]
        public void Render_WithPerson_SuccessfulTests(string templateText, string resultText, string reason)
        {
            var personWrapper = new { person };
            var expressTemplateManager = Factory.CreateExpressTemplateManager();

            var result = expressTemplateManager.Render(templateText, personWrapper);

            result.ShouldBe(resultText, reason);
        }

        [Theory]
        [InlineData(@"{{ 9 + 8 }}", "17", "add intergers")]
        [InlineData(@"{{ 9.1 + 8.2 }}", "17.3", "add double")]
        [InlineData(@"{{ 9 + 8.3 }}", "17.3", "add different number types")]
        [InlineData(@"{{ (9 + 8) }}", "17", "brackets")]
        [InlineData(@"{{ Test.double 8 }}", "16", "convert")]
        public void AddExpression(string templateText, string resultText, string reason)
        {
            var expressTemplateManager = Factory.CreateExpressTemplateManager();
            var result = expressTemplateManager.Render(templateText, new { });
            result.ShouldBe(resultText, reason);
        }

        [Fact]
        public void DuplicateTemplate_WithDifferentTargetType()
        {
            var expressTemplateManager = Factory.CreateExpressTemplateManager();

            var resultNumber = expressTemplateManager.Render("{{ abc }}", new { abc= 5});
            var resultstring = expressTemplateManager.Render("{{ abc }}", new { abc = "6" });

            resultNumber.ShouldBe("5");
            resultstring.ShouldBe("6");
        }
        [Fact]
        public void InvalidTemplate_EmptyStringReturned()
        {
            var expressTemplateManager = Factory.CreateExpressTemplateManager();

            var resultNumber = expressTemplateManager.Render("{{ ssabc }}", new { abc = 5 });
            
            resultNumber.ShouldBe(string.Empty);
        }

        [Fact]
        public void InvalidStatement_ReturnsPartialSatement()
        {
            var expressTemplateManager = Factory.CreateExpressTemplateManager();

            var resultNumber = expressTemplateManager.Render("hello{{ ssabc }}", new { abc = 5 });

            resultNumber.ShouldBe("hello");
        }
    }
}
