using ScribanExpress.Library;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ScribanExpress.UnitTests.Library
{
    public class TypeConverterTests
    {
        private readonly TypeConverter typeConverter;

        public TypeConverterTests()
        {
            typeConverter = new TypeConverter();
        }

        [Fact]
        public void CanConvert_IntToDouble()
        {
            typeConverter.CanConvert(typeof(int), typeof(double))
                .ShouldBeTrue();
        }

        [Fact]
        public void Convert_IntToInt_ReturnsFalse()
        {
            typeConverter.CanConvert(typeof(int), typeof(int))
                .ShouldBeFalse();
        }
    }
}
