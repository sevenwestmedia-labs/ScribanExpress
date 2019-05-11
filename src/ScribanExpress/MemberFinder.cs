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
                ?? FindMemberInfo(type, memberName.Replace("_", string.Empty), arguments);
        }

        private MemberInfo FindMemberInfo(Type type, string memberName, IEnumerable<Type> arguments)
        {
            var property = ReflectionHelpers.GetProperty(type, memberName);
            if (property != null)
            {
                return property;
            }

            var methodInfo = ReflectionHelpers.GetMethod(type, memberName, arguments);
            if (methodInfo != null)
            {
                return methodInfo;
            }

            methodInfo = MatchMethodWithDefaultParameters(type, memberName, arguments.ToNullSafe().ToList());
            if (methodInfo != null)
            {
                return methodInfo;
            }

            var genericMethodInfo = this.MatchGenericMethod(type, memberName, arguments.ToNullSafe().ToList());

            if (genericMethodInfo != null)
            {
                return genericMethodInfo;
            }

            //search generics
            //https://stackoverflow.com/questions/5218395/reflection-how-to-get-a-generic-method
            //https://docs.microsoft.com/en-us/dotnet/framework/reflection-and-codedom/how-to-examine-and-instantiate-generic-types-with-reflection

            return null;
        }

        // https://stackoverflow.com/questions/2421994/invoking-methods-with-optional-parameters-through-reflection
        private MethodInfo MatchMethodWithDefaultParameters(Type type, string memberName, IList<Type> arguments)
        {
            var potentialMatches = type.GetMethods(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.OptionalParamBinding)
             .Where(m => m.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase))
             .Where(m => !m.IsGenericMethod); // TODO: we don't support Generic combined with Default values... yet

            foreach (MethodInfo methodInfo in potentialMatches)
            {
                var parameters = methodInfo.GetParameters();

                if (DoArgsMatchMethod(methodInfo, arguments))
                {
                    return methodInfo;
                }
            }
            return null;
        }


        //https://docs.microsoft.com/en-us/dotnet/api/system.type.isassignablefrom?view=netcore-2.2
        private bool DoArgsMatchMethod(MethodInfo methodInfo, IList<Type> arguments)
        {
            var parameters = methodInfo.GetParameters();

            // can't have more arguments than a method parameters
            if (arguments.Count() > parameters.Count())
            {
                return false;
            }

            foreach (var (index, parameter) in parameters.GetIndexedEnumerable())
            {
                var argument = arguments.ElementAtOrDefault(index);

                if (argument == null && parameter.IsOptional)
                {
                    continue;
                }
                
                if (!parameter.ParameterType.IsAssignableFrom(argument))
                {
                    return false;
                }
            }

            return true;
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
