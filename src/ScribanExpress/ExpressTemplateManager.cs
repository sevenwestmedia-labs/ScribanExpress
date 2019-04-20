using Scriban;
using ScribanExpress.Abstractions;
using ScribanExpress.Functions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress
{
    public class ExpressTemplateManager: IExpressTemplateManager
    {
        private readonly IDictionary<string, object> functionary;
        private readonly ExpressionGenerator expressionGenerator;
        private readonly FunctionLibary functionLibary;
        public ExpressTemplateManager()
        {
            functionLibary = new FunctionLibary();
            expressionGenerator = new ExpressionGenerator();
            functionary = new ConcurrentDictionary<string, object>();
        }
        public string Render<T>(string templateText, T value)
        {
            return GetFunc<T>(templateText)(value);
        }


        private Func<T, string> GetFunc<T>(string templateText)
        {
            if (!functionary.ContainsKey(templateText))
            {
                var template = Template.Parse(templateText, null, null, null);
                var expression = expressionGenerator.Generate<T, FunctionLibary>(template.Page.Body);
                var compiled =  expression.Compile();

                functionary.Add(templateText, compiled);
            }

            return MapFunction(functionary[templateText] as Action<StringBuilder,T, FunctionLibary>, functionLibary);
        }


        private Func<T, string> MapFunction<T,Y>(Action<StringBuilder,T,Y> input, Y libary)
        {
            Func<T, string> returnFunc = x => {
                StringBuilder sb = new StringBuilder();
                //try catch here? or per statement?
                input(sb, x, libary);
                return sb.ToString();
            };
            return returnFunc;
        }
    }
}
