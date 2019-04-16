using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.UnitTests.Models
{
    public class TestLibrary
    {
        public TestLibrary()
        {
            Deep = new Deep();
        }
        public string Repeat(string value) => $"{value}{value}";

        public string TestProperty => $"Bill";

        public string MultString(string a, string b)
        {
            return a + b;
        }

        public Deep Deep { get; set; }
        public string AppendConst(string value)
        {
            return value;
        }
    }
    public class Deep
    {
        public string Append(string value, string other)
        {
            return value + other;
        }
    }
}
