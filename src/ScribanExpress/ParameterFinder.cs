using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ScribanExpress
{
    public class ParameterFinder
    {
        private readonly Stack<ParameterExpression> parameterStack;

        public ParameterFinder()
        {
            parameterStack = new Stack<ParameterExpression>();
        }

        public ParameterExpression Find(string propertyName)
        {
            foreach (var parameterExpression in parameterStack)
            {
                var propertyExists = parameterExpression.Type.GetProperty(propertyName, BindingFlags.Public  | BindingFlags.Instance | BindingFlags.IgnoreCase) != null;
                if (propertyExists)
                {
                    return parameterExpression;
                }

            }
            return null;
        }

        public void AddType(ParameterExpression parameterExpression)
        {
            parameterStack.Push(parameterExpression);
        }
    }
}
