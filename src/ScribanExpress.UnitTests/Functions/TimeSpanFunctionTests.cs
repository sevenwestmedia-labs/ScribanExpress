using ScribanExpress.Abstractions;
using ScribanExpress.UnitTests.Helpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ScribanExpress.UnitTests.Functions
{
   public class TimeSpanFunctionTests
    {
        [Theory]
        [InlineData(@"{{ (TimeSpan.fromdays 5).days }}", "5")]
        public void Function_Tests(string templateText, string resultText)
        {
            IExpressTemplateManager expressTemplateManager = Factory.CreateExpressTemplateManager();
            var result = expressTemplateManager.Render(templateText, new { });

            result.ShouldBe(resultText);
        }
    }
}
