using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScribanExpress.Extensions;
using Shouldly;
using Xunit;

namespace ScribanExpress.UnitTests.Extensions
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void LeftZip_LeftHasMore_RightIsNull()
        {
            var strings = new List<string> { "one", "two", "three" };
            var rightStrings = new List<string> { "one", "two" };


            var result = strings
                            .LeftZip(rightStrings)
                            .ToList();

            result.Count().ShouldBe(3);
            result[2].Right.ShouldBeNull();
            result[2].Left.ShouldBe("three");
        }
    }
}
