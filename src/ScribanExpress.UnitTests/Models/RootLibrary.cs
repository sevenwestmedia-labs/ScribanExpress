using ScribanExpress.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.UnitTests.Models
{
    public class RootLibary : StandardLibrary
    {
        public RootLibary()
        {
            Test = new TestLibrary();
            Debug = new DebugFunctions();
        }
        public TestLibrary Test { get;  }

        public DebugFunctions Debug { get; }
    }
}
