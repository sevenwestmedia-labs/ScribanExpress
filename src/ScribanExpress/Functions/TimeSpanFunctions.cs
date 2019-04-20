using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.Functions
{
    public class TimeSpanFunctions: Scriban.Functions.TimeSpanFunctions
    {
        public static TimeSpan FromDays(int days)
        {
            return TimeSpan.FromDays(days);
        }
    }
}
