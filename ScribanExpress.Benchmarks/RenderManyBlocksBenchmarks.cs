using BenchmarkDotNet.Attributes;
using Scriban;
using ScribanExpress.Abstractions;
using ScribanExpress.Benchmarks.Comparison.ThirdParty.Razor;
using ScribanExpress.Functions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace ScribanExpress.Benchmarks
{
    public class RenderManyBlocksBenchmarks
    {
        /* 2019-04-21 Razor
        |                  Method |      Mean |     Error |    StdDev | Ratio |
        |------------------------ |----------:|----------:|----------:|------:|
        | ExpressRenderManyBlocks |  1.251 us | 0.0073 us | 0.0061 us |  0.02 |
        | ScribanRenderManyBlocks | 60.528 us | 0.4119 us | 0.3853 us |  1.00 |
        |   RazorRenderManyBlocks |  2.071 us | 0.0167 us | 0.0156 us |  0.03 |
        */
        /* 2019-04-20 StringBuilder
        |                  Method |      Mean |     Error |    StdDev | Ratio |
        |------------------------ |----------:|----------:|----------:|------:|
        | ExpressRenderManyBlocks |  1.257 us | 0.0084 us | 0.0078 us |  0.02 |
        | ScribanRenderManyBlocks | 63.220 us | 0.8285 us | 0.7750 us |  1.00 |
        */
        /* 2019-04-19 Concat
        |                  Method |      Mean |     Error |    StdDev | Ratio |
        |------------------------ |----------:|----------:|----------:|------:|
        | ExpressRenderManyBlocks |  2.730 us | 0.0329 us | 0.0307 us |  0.04 |
        | ScribanRenderManyBlocks | 62.657 us | 0.8923 us | 0.8346 us |  1.00 |
        */

        private readonly IExpressTemplateManager expressTemplateManager;
        private readonly Template scribanTemplate;
        private readonly RazorTemplatePage _razorTemplate;
        private readonly string templateText;

        public RenderManyBlocksBenchmarks()
        {
            templateText = "Hello {{name}} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }} blah {{ name }}";
            expressTemplateManager = new ExpressTemplateManager<FunctionLibary>(new FunctionLibary());
            scribanTemplate = Template.Parse(templateText);
            _razorTemplate = RazorBuilder.Compile(@"Hello @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name blah @Model.Name");
        }

        [Benchmark]
        public string ExpressRenderManyBlocks()
        {
            return expressTemplateManager.Render(templateText, new { Name = "World" });
        }

        [Benchmark(Baseline = true)]
        public string ScribanRenderManyBlocks()
        {
            return scribanTemplate.Render(new { Name = "World" });
        }


        [Benchmark]
        public string RazorRenderManyBlocks()
        {
            var writer = new StringWriter();

            dynamic expando = new ExpandoObject();
            expando.Name = "World";
            _razorTemplate.Output = writer;
            _razorTemplate.Model = expando;
            _razorTemplate.Execute();
            
            return writer.ToString();
        }
    }
}
