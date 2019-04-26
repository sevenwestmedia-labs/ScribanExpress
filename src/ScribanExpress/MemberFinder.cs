using ScribanExpress.Extensions;
using ScribanExpress.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ScribanExpress
{
    public class MemberFinder
    {
        public MemberInfo FindMember(Type type, string memberName, IEnumerable<Type> arguments)
        {
            return FindMemberInfo(type, memberName, arguments)
                // Use Scriban Naming Convention
                ?? FindMemberInfo(type, memberName.Replace("_",string.Empty), arguments); 
        }

        private MemberInfo FindMemberInfo(Type type, string memberName, IEnumerable<Type> arguments)
        {
            var property = ExpressionHelpers.GetProperty(type, memberName);
            if (property != null)
            {
                return property;
            }
            
            var methodInfo = ExpressionHelpers.GetMethod(type, memberName, arguments);
            if (methodInfo != null)
            {
                return methodInfo;
            }

            var genericMethodInfo = this.MatchGenericMethod(type, memberName, arguments.ToNullSafe().ToList());

            if (genericMethodInfo != null)
            {
                return genericMethodInfo;
            }


            return null;
        }


        private MethodInfo MatchGenericMethod(Type type, string memberName, IList<Type> arguments)
        {
            var potentialMatches = type.GetMethods(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                            .Where(m => m.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase))
                            .Where(m => m.IsGenericMethod)
                            .Where(m => m.GetParameters().Length == arguments.Count());


            foreach (var genericMethodInfo in potentialMatches)
            {
                var genericVersion = ResolveGenericMethodParameters(genericMethodInfo);
                if (genericVersion != null)
                {
                    return genericVersion;
                }
            }

            // check to see if the supplied types can be used to resolve the genericParamters
            MethodInfo ResolveGenericMethodParameters(MethodInfo potentialmatch)
            {

                //Match Generic arguments with the method arguments to find possible types
                //connect a <T> to a arguments (T item)
                DictionaryList<Type, Type> genericTargetTypes = new DictionaryList<Type, Type>();
                var methodParameters = potentialmatch.GetParameters();
                for (int i = 0; i < methodParameters.Count(); i++)
                {
                    if (methodParameters[i].ParameterType.IsGenericParameter)
                    {
                        genericTargetTypes.Add(methodParameters[i].ParameterType, arguments[i]);
                    }

                    if (methodParameters[i].ParameterType.IsGenericType)
                    {
                        genericTargetTypes.Add(methodParameters[i].ParameterType.GenericTypeArguments[0], arguments[i].GenericTypeArguments[0]);
                    }
                }
                //todo We need to check the the supplied types implement the generic where constraints

                var genericParameters = potentialmatch.GetGenericArguments();

                // todo we need to look at all posssible types and find their highest common ancestor
                // currently just assuming the first
                var typeArguments = genericParameters.Select(g => genericTargetTypes.Get(g).First());
                return potentialmatch.MakeGenericMethod(typeArguments.ToArray());
            }

            return null;
        }
    }
}
