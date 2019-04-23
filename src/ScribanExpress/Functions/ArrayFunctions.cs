using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.Functions
{
   public class ArrayFunctions
    {
        public static string Join(IEnumerable list, string delimiter)
        {
            if (list == null)
            {
                return string.Empty;
            }

            var text = new StringBuilder();
            bool afterFirst = false;
            foreach (var obj in list)
            {
                if (afterFirst)
                {
                    text.Append(delimiter);
                }
                text.Append(obj);
                afterFirst = true;
            }
            return text.ToString();
        }
    }
}
