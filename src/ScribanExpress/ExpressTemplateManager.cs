using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ExpressTemplateManager<L>> logger;
        private readonly L standardLibrary;

        public ExpressTemplateManager(ILogger<ExpressTemplateManager<L>> logger, L standardLibrary, StatementGenerator statementGenerator)
        {
            this.logger = logger;
            this.standardLibrary = standardLibrary;
            this.statementGenerator = statementGenerator;
            functionary = new ConcurrentDictionary<string, object>();
        }
        public string Render<T>(string templateText, T value)
        {
            var expressContext = GetExpressContext<T>(templateText);
            return MapFunction(expressContext.CompiledTemplate, standardLibrary)
                            (value);
        }

        public string Render<T>(ExpressContext<T, L> expressContext, T value)
        {
            return MapFunction(expressContext.CompiledTemplate, standardLibrary)
                            (value);
        }

        public ExpressContext<T, L> GetExpressContext<T>(string templateText)
        {
            Func<string, ExpressContext<T, L>> CompileTemplate = _ =>
            {
                ExpressContext<T, L> expressContext = new ExpressContext<T, L>();
                logger.LogInformation("Compiling {templateText} for {type}", templateText, typeof(T));
                try
                {
                    var template = Template.Parse(templateText, null, null, null);
                    expressContext.Messages.AddRange(template.Messages);

                    if (template.HasErrors)
                    {
                        logger.LogError("Scriban Parsing Failed on {templateText} for {type}", templateText, typeof(T));
                        expressContext.CompiledTemplate = (sb, target, library) => { };
                        return expressContext;
                    }
                    else
                    {
                        logger.LogInformation("Scriban Parsing Succeded on {templateText} for {type}", templateText, typeof(T));
                    }

                    var expression = statementGenerator.Generate<T, L>(expressContext, template.Page.Body);
                    expressContext.CompiledTemplate = expression.Compile();
                    return expressContext;
                }
                catch (Exception ex)
                {
                    // effectively swallow exceptions here,
                    // as compilation could take a long time and we don't want to overload the server with repeated failing compiles

                    logger.LogError(ex, "Compilation Failed on {templateText} for {type}", templateText, typeof(T));

                    Action<StringBuilder, T, L> emptyAction = (sb, target, library) => { };
                    expressContext.CompiledTemplate = emptyAction;
                    return expressContext;
                }
            };

            string cacheKey = typeof(T) + templateText;
            var compiltedTemplate = functionary.GetOrAdd(cacheKey, CompileTemplate) as ExpressContext<T, L>;
            return compiltedTemplate;
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
