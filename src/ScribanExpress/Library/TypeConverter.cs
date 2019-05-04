using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.Library
{
    public class TypeConverter
    {
        private static readonly List<Type> convertableFromInt = new List<Type> { typeof(long), typeof(float), typeof(double), typeof(decimal) };

        // TODO: Consider implementing https://stackoverflow.com/questions/17676838/how-to-check-if-type-can-be-converted-to-another-type-in-c-sharp
        public bool CanConvert(Type from, Type to)
        {
            return ImplicitConvert(from, to);
        }

        // TODO: add more implicit Types
        //https://docs.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2012/y5b434w4(v%3dvs.110)
        private bool ImplicitConvert(Type from, Type to)
        {
            if (from == typeof(int))
            {
                if (convertableFromInt.Contains(to))
                {
                    return true;
                }

            }
            return false;
        }
    }
}
