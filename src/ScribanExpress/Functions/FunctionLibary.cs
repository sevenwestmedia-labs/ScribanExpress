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
            TimeSpan = new TimeSpanFunctions();
        }

        public DateTimeFunctions Date { get; }

        public StringFunctions String { get; }

        public TimeSpanFunctions TimeSpan { get; }
    }
}
