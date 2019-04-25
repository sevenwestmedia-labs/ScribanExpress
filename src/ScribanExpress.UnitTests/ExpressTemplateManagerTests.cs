using ScribanExpress.UnitTests.Helpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ScribanExpress.UnitTests
{
    public class ExpressTemplateManagerTests
    {

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
