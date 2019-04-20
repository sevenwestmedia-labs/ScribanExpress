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
            ExpressTemplateManager expressTemplateManager = new ExpressTemplateManager();
            var result = expressTemplateManager.Render(templateText, new { });

            result.ShouldBe(resultText);
        }
    }
}
