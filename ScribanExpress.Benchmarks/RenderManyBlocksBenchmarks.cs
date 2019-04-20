using BenchmarkDotNet.Attributes;
using Scriban;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.Benchmarks
{
    public class RenderManyBlocksBenchmarks
    {
        /* 2019-04-20 StringBuilder
        |                  Method |      Mean |     Error |    StdDev | Ratio |
        |------------------------ |----------:|----------:|----------:|------:|
        | ExplateRenderManyBlocks |  1.257 us | 0.0084 us | 0.0078 us |  0.02 |
        | ScribanRenderManyBlocks | 63.220 us | 0.8285 us | 0.7750 us |  1.00 |
        */
        /* 2019-04-19 Concat
        |                  Method |      Mean |     Error |    StdDev | Ratio |
        |------------------------ |----------:|----------:|----------:|------:|
        | ExplateRenderManyBlocks |  2.730 us | 0.0329 us | 0.0307 us |  0.04 |
        | ScribanRenderManyBlocks | 62.657 us | 0.8923 us | 0.8346 us |  1.00 |
        */

        private readonly ExpressTemplateManager expressTemplateManager;
        private readonly Template scribanTemplate;
        private readonly string templateText;

        public RenderManyBlocksBenchmarks()
        {
            templateText = "Hello {{name}} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }}";
            expressTemplateManager = new ExpressTemplateManager();
            scribanTemplate = Template.Parse(templateText);
        }

        [Benchmark]
        public string ExplateRenderManyBlocks()
        {
            // | RenderManyBlocks | 2.731 us | 0.0204 us | 0.0180 us |
            return expressTemplateManager.Render(templateText, new { Name = "World" });
        }

        [Benchmark(Baseline = true)]
        public string ScribanRenderManyBlocks()
        {
            return scribanTemplate.Render(new { Name = "World" });
        }
    }
}
