using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace ScribanExpress.UnitTests
{
   public class ParameterFinderTests
    {
        [Fact]
        public void FindProperty()
        {
            var finder = new ParameterFinder();
            var x = new { Name="someName" };
            finder.AddType(Expression.Parameter(x.GetType()));

            var result = finder.GetProperty("Name");

            result.ShouldNotBeNull();
        }

        [Fact]
        public void FindProperty_WithDifferentCasing()
        {
            var finder = new ParameterFinder();
            var x = new { Name = "someName" };
            finder.AddType(Expression.Parameter(x.GetType()));

            var result = finder.GetProperty("naMe");

            result.ShouldNotBeNull();
        }
    }
}
