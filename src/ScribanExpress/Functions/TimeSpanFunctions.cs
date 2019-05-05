using System;
using System.Collections.Generic;
using System.Text;
using Original = Scriban.Functions;

namespace ScribanExpress.Functions
{
    public class TimeSpanFunctions
    {
        public static TimeSpan FromDays(double days) => Original.TimeSpanFunctions.FromDays(days);
    
        public static TimeSpan FromHours(double hours) => Original.TimeSpanFunctions.FromHours(hours);
    
        public static TimeSpan FromMilliseconds(double millis) => Original.TimeSpanFunctions.FromMilliseconds(millis);
    
        public static TimeSpan FromMinutes(double minutes) => Original.TimeSpanFunctions.FromMinutes(minutes);
    
        public static TimeSpan FromSeconds(double seconds) => Original.TimeSpanFunctions.FromSeconds(seconds);
    }
}
