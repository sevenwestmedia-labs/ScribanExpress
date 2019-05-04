using System;
using System.Collections.Generic;
using System.Text;
using Original = Scriban.Functions;

namespace ScribanExpress.Functions
{
   public class DateFunctions
    {

        public static DateTime AddDays(DateTime date, double days)=> Original.DateTimeFunctions.AddDays(date, days);

        public static DateTime AddHours(DateTime date, double hours) => Original.DateTimeFunctions.AddHours(date, hours);
    }
}
