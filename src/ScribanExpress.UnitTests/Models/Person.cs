using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.UnitTests.Models
{
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Company Company { get; set; }

        //public DateTime Birthday { get; set; }
    }
}
