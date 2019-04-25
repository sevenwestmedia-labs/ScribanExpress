using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ScribanExpress.Functions
{
    public class DebugFunctions
    {
        public string ShowMembers<T>(T value)
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
