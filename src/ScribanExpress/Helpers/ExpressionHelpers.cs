using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScribanExpress.Helpers
{
    public class ExpressionHelpers
    {
        public static bool PropertyExists(Type type, string propertyName)
        {
            return type.GetProperty(propertyName) != null;
        }

        public static bool MethodExists(Type type, string methodName, IEnumerable<Type> argTypes)
        {
            return type.GetMethod(methodName, (argTypes ?? Enumerable.Empty<Type>()).ToArray() ) != null;
        }
    }
}
