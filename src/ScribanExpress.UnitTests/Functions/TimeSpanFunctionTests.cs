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
            ExpressTemplateManager expressTemplateManager = new ExpressTemplateManager();
            var result = expressTemplateManager.Render(templateText, new { });

            result.ShouldBe(resultText);
        }
    }
}
