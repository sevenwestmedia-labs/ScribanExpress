using Scriban;
using ScribanExpress.Abstractions;
using ScribanExpress.Functions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress
{
    public class ExpressTemplateManager<L> : IExpressTemplateManager where L : StandardLibrary
    {
        private readonly ConcurrentDictionary<string, object> functionary;
        private readonly StatementGenerator statementGenerator;
        private readonly L standardLibrary;

        public ExpressTemplateManager(L standardLibrary)
        {
            this.standardLibrary = standardLibrary;
            statementGenerator = new StatementGenerator();
            functionary = new ConcurrentDictionary<string, object>();
        }
        public string Render<T>(string templateText, T value)
        {
            return GetFunc<T>(templateText)(value);
        }

        private Func<T, string> GetFunc<T>(string templateText)
        {
            Func<string, object> Compiler = (_) =>
                {
                    var template = Template.Parse(templateText, null, null, null);
                    var expression = statementGenerator.Generate<T, L>(template.Page.Body);
                    var compiled = expression.Compile();
                    return compiled;

                };

            string cacheKey = typeof(T) + templateText;
            var compiledItem = functionary.GetOrAdd(cacheKey, Compiler);

            return MapFunction(compiledItem as Action<StringBuilder, T, L>, standardLibrary);
        }

        private Func<T, string> MapFunction<T, Y>(Action<StringBuilder, T, Y> input, Y libary)
        {
            Func<T, string> returnFunc = x =>
            {
                StringBuilder sb = new StringBuilder();
                //try catch here? or per statement?
                input(sb, x, libary);
                return sb.ToString();
            };
            return returnFunc;
        }
    }
}
