using ScribanExpress.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ScribanExpress
{
    public class ParameterFinder : IDisposable  
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ParameterFinder()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
