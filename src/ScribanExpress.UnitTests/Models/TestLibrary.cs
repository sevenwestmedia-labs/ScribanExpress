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

        public string PrefixPerson(string prefix, Person person)
        {
            return $"{prefix} {person.LastName}";
        }

        public static string StaticHello(string value) => $"Hello{value}Static";


        public Deep Deep { get; set; }
        public string AppendConst(string value)
        {
            return value;
        }

        public double Double(double item) 
        {
            return item * 2;
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
