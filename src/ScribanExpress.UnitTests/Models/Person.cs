using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.UnitTests.Models
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Company Company { get; set; }

        public TitleType Title { get; set; }

        public string DontSet { get; set; }
        //public DateTime Birthday { get; set; }
    }
}
