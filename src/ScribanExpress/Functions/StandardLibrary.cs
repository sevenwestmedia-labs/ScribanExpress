using Scriban.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.Functions
{
    public class StandardLibrary
    {
        public StandardLibrary()
        {
            Date = new DateFunctions();
            String = new StringFunctions();
            TimeSpan = new TimeSpanFunctions();
            Array = new ArrayFunctions();
        }

        public DateFunctions Date { get; }

        public StringFunctions String { get; }

        public TimeSpanFunctions TimeSpan { get; }
        public ArrayFunctions Array { get; }
    }
}
