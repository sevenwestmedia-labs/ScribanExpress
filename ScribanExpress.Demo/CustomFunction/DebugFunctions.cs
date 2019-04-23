using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public string ShowMembers(object value)
        {
            var members = (value.GetType().GetProperties() as MemberInfo[])
                .Union(value.GetType().GetMethods().Where(m => !m.IsSpecialName));

            return members
                .Select(x => x.Name)
                .Aggregate((res, item) => res + " <br/> " + item)
                ;
        }
    }
}
