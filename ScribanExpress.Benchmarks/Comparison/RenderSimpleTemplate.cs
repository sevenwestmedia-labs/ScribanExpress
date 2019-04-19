using BenchmarkDotNet.Attributes;
using Scriban;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScribanExpress.Benchmarks.Comparison
{
    public class RenderSimpleTemplate
    {
        private readonly ExpressTemplateManager expressTemplateManager;
        private readonly Template scribanTemplate;
        public RenderSimpleTemplate()
        {
            expressTemplateManager = new ExpressTemplateManager();
            scribanTemplate = Template.Parse("Hello {{name}}!");
        }
        [Benchmark(Description = "Express Hello 50", Baseline = true)]
        public string ExpressHello50()
        {
            var writer = new StringWriter();
            for (int i = 0; i < 50; i++)
            {
                var result = expressTemplateManager.Render("Hello {{name}}!", new { Name = "World" });
                writer.Write(result);
            }

            return writer.ToString();
        }

        [Benchmark(Description = "Scriban Hello 50")]
        public string ScribanHello50()
        {
            var writer = new StringWriter();
            for (int i = 0; i < 50; i++)
            {
                var result = scribanTemplate.Render(new { Name = "World" });
                writer.Write(result);
            }
            return writer.ToString();
        }
    }
}
