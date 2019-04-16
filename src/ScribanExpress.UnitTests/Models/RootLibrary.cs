using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.UnitTests.Models
{
    public class RootLibary
    {
        public RootLibary()
        {
            Test = new TestLibrary();
        }
        public TestLibrary Test { get;  }
    }
}
