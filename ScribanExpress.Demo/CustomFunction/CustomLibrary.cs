using ScribanExpress.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScribanExpress.Demo.CustomFunction
{
    public class CustomLibrary : StandardLibrary
    {
        public CustomLibrary()
        {
            Debug = new DebugFunctions();
        }

        public DebugFunctions Debug { get; }
    }

}
