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

        public static DateTime AddMilliseconds(DateTime date, double millis) => Original.DateTimeFunctions.AddMilliseconds(date, millis);

        public static DateTime AddMinutes(DateTime date, double minutes) => Original.DateTimeFunctions.AddMinutes(date, minutes);

        public static DateTime AddMonths(DateTime date, int months) => Original.DateTimeFunctions.AddMonths(date, months);

        public static DateTime AddSeconds(DateTime date, double seconds) => Original.DateTimeFunctions.AddSeconds(date, seconds);

        public static DateTime AddYears(DateTime date, int years) => Original.DateTimeFunctions.AddYears(date, years);

        public static DateTime Now() => Original.DateTimeFunctions.Now();


    }
}
