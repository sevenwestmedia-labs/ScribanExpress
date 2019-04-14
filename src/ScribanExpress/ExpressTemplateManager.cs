using Scriban;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress
{
    public class ExpressTemplateManager
    {
        private readonly IDictionary<string, object> functionary;
        private readonly ExpressionGenerator expressionGenerator;
        public ExpressTemplateManager()
        {
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
                var expression = expressionGenerator.Generate<T,object>(template.Page.Body);
                var compiled =  expression.Compile();

                functionary.Add(templateText, compiled);
            }


            return MapFunction(functionary[templateText] as Func<T, object,string>, null);
        }


        private Func<T, string> MapFunction<T,Y>(Func<T,Y, string> input, Y libary)
        {
            Func<T, string> returnFunc = x => input(x, libary);
            return returnFunc;
        }
    }
}
