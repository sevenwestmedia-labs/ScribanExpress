using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ScribanExpress
{
    public class ParameterFinder
    {
        private readonly ParameterFinder parentFinder;
        private readonly Stack<ParameterExpression> parameterStack;
        private readonly Stack<ParameterExpression> localStack;
        public ParameterFinder()
        {
            parameterStack = new Stack<ParameterExpression>();
            localStack = new Stack<ParameterExpression>();
        }

        private ParameterFinder(ParameterFinder parent) : this()
        {
            this.parentFinder = parent;
        }
        public ParameterExpression Find(string propertyName)
        {

            return FindLocalVariable(propertyName) ?? FindGlobalObject(propertyName);
        }

        public ParameterExpression FindLocalVariable(string propertyName)
        {
            foreach (var parameterExpression in localStack)
            {
                if (parameterExpression.Name == propertyName)
                {
                    return parameterExpression;
                }
            }
            if (parentFinder != null)
            {
                return parentFinder.FindLocalVariable(propertyName);
            }
            return null;
        }

        public ParameterExpression FindGlobalObject(string propertyName)
        {
            foreach (var parameterExpression in parameterStack)
            {
                var propertyExists = parameterExpression.Type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) != null;
                if (propertyExists)
                {
                    return parameterExpression;
                }

            }

            if (parentFinder != null)
            {
                return parentFinder.FindGlobalObject(propertyName);
            }
            return null;
        }

        public void AddType(ParameterExpression parameterExpression)
        {
            parameterStack.Push(parameterExpression);
        }

        public void AddLocalVariable(ParameterExpression parameterExpression)
        {
            localStack.Push(parameterExpression);
        }

        public ParameterFinder CreateScope()
        {
            return new ParameterFinder(this);
        }
    }
}
