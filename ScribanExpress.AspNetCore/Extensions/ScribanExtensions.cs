using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ScribanExpress.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.AspNetCore.Extensions
{
    public static class ScribanExtensions
    {
        public static IServiceCollection AddScribanExpress(this IServiceCollection collection)
        {
            collection.TryAddSingleton<IExpressTemplateManager, ExpressTemplateManager>();
            return collection;
        }
    }
}
