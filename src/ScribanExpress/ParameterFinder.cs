using ScribanExpress.Helpers;
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

        public Expression GetProperty(string propertyName)
        {
            var local = FindLocalVariable(propertyName);
            if (local != null)
            {
                return local;
            }
            else {
                var globalParam = FindGlobalObject(propertyName);
                return Expression.Property(globalParam, propertyName);
            }
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
                var propertyExists = ExpressionHelpers.GetProperty(parameterExpression.Type, propertyName) != null;
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
