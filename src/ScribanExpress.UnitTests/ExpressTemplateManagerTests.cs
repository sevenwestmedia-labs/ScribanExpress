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
    }
}
