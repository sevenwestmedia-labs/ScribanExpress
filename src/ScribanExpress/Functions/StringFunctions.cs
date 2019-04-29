using System;
using System.Collections.Generic;
using System.Text;
using Original = Scriban.Functions;
namespace ScribanExpress.Functions
{
    public class StringFunctions
    {
        public static string Append(string text, string with) => Original.StringFunctions.Append(text, with);





        public static string Capitalize(string text) => Original.StringFunctions.Capitalize(text);

        public static string CapitalizeWords(string text) => Original.StringFunctions.Capitalizewords(text);

        public static bool Contains(string text, string value) => Original.StringFunctions.Contains(text, value);

        public static string Downcase(string text) => Original.StringFunctions.Downcase(text);

        public static bool EndsWith(string text, string value) => Original.StringFunctions.EndsWith(text, value);

        

        public static string Pluralize(int number, string singular, string plural) => Original.StringFunctions.Pluralize(number, singular, plural);
        public static string Prepend(string text, string by) => Original.StringFunctions.Prepend(text, by);
        public string Remove(string text, string remove) => Original.StringFunctions.Remove(text, remove);
        public static string RemoveFirst(string text, string remove) => Original.StringFunctions.RemoveFirst(text, remove);


        public static string Truncate(string text, int length) => Original.StringFunctions.Truncate(text, length);

    }
}
