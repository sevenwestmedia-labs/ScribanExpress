using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.Functions
{
    public class StringFunctions : Scriban.Functions.StringFunctions
    {
        public static string Truncate(string text, int length)
        {
            return Scriban.Functions.StringFunctions.Truncate(text, length);
        }
    }
}
