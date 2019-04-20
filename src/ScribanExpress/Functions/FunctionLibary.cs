using Scriban.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.Functions
{
    public class FunctionLibary
    {
        public FunctionLibary()
        {
            Date = new DateTimeFunctions();
            String = new StringFunctions();
        }

        public DateTimeFunctions Date { get; }

        public StringFunctions String { get; }
    }
}
