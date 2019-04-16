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

        public string Swap(string a, string b)
        {
            return  b + a;
        }

        public string ReturnHello() => "Hello";

        public string GetPersonName(Person person)
        {
            return $"{person.FirstName} {person.LastName}";
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
