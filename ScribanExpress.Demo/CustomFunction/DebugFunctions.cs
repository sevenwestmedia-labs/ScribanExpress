using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScribanExpress.Demo.CustomFunction
{
    public class DebugFunctions
    {
        public string ShowProperties(object value)
        {
            return value.GetType()
                .GetProperties()
                .Select(x => x.Name)
                .Aggregate((res, item) => res + " <br/> " + item)
                ;
        }
    }
}
