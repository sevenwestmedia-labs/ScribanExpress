using ScribanExpress.Abstractions;
using ScribanExpress.UnitTests.Helpers;
using ScribanExpress.UnitTests.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ScribanExpress.UnitTests.Functions
{
    public class ArrayFunctionsTests
    {

        List<Company> Companies;
        public ArrayFunctionsTests()
        {
            Companies = new List<Company> { new Company { Title = "abc" }, new Company { Title = "second" } };
        }

        [Theory]
        [InlineData(@"{{ Companies[0].Title  }}", "abc")]
        [InlineData(@"{{ Companies[1].Title  }}", "second")]
        public void Array_Tests(string templateText, string resultText)
        {
            IExpressTemplateManager expressTemplateManager = Factory.CreateExpressTemplateManager();
            var result = expressTemplateManager.Render(templateText, new { Companies });

            result.ShouldBe(resultText);
        }

    }
}
