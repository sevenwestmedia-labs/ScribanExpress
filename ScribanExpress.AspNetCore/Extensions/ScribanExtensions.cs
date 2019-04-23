using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ScribanExpress.Abstractions;
using ScribanExpress.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.AspNetCore.Extensions
{
    public static class ScribanExtensions
    {
        public static IServiceCollection AddScribanExpress(this IServiceCollection collection)
        {
            collection.TryAddSingleton<IExpressTemplateManager, ExpressTemplateManager<FunctionLibary>>();
            collection.TryAddSingleton<FunctionLibary>();
            return collection;
        }

        public static IServiceCollection AddScribanExpress<T>(this IServiceCollection collection) where T : FunctionLibary
        {
            collection.TryAddSingleton<IExpressTemplateManager, ExpressTemplateManager<T>>();
            collection.TryAddSingleton<T>();
            return collection;
        }
    }
}
