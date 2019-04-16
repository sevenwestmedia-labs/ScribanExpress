using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.Helpers
{
    public class ExpressionHelpers
    {
        public static bool PropertyExists(Type type, string propertyName)
        {
            return type.GetProperty(propertyName) != null;
        }

        public static bool MethodExists(Type type, string methodName)
        {
            return type.GetMethod(methodName) != null;
        }
    }
}
