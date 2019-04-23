using ScribanExpress.Abstractions;
using ScribanExpress.Functions;
using ScribanExpress.UnitTests.Helpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ScribanExpress.UnitTests.Functions
{
    public class StringFunctionsTests
    {
        public StringFunctionsTests()
        {
        }

        [Theory]
        [InlineData(@"{{ ""test"" | string.capitalize }}", "Test")]
        public void Function_Tests(string templateText, string resultText)
        {
            IExpressTemplateManager expressTemplateManager = Factory.CreateExpressTemplateManager();
            var result = expressTemplateManager.Render(templateText, new { });

            result.ShouldBe(resultText);
        }
    }
}
