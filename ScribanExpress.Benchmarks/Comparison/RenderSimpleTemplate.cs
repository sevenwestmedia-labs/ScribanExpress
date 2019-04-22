using BenchmarkDotNet.Attributes;
using Scriban;
using ScribanExpress.Benchmarks.Comparison.ThirdParty.Razor;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace ScribanExpress.Benchmarks.Comparison
{
    public class RenderSimpleTemplate
    {   
        /* 2019-04-21 Razor
        |             Method |         Mean |      Error |     StdDev | Ratio |
        |------------------- |-------------:|-----------:|-----------:|------:|
        | 'Express Hello 50' |     8.125 us |  0.0283 us |  0.0265 us | 0.003 |
        | 'Scriban Hello 50' | 2,930.580 us | 37.8549 us | 35.4095 us | 1.000 |
        |   'Razor Hello 50' |    26.461 us |  0.5109 us |  0.5018 us | 0.009 |
        */
        /* 2019-04-20 Stringbuilder
        |             Method |         Mean |      Error |     StdDev | Ratio |
        |------------------- |-------------:|-----------:|-----------:|------:|
        | 'Express Hello 50' |     8.291 us |  0.0649 us |  0.0607 us | 0.003 |
        | 'Scriban Hello 50' | 2,860.825 us | 25.9733 us | 24.2954 us | 1.000 |
        */
        /* 2019-04-19 Concat
        |             Method |         Mean |      Error |     StdDev | Ratio |
        |------------------- |-------------:|-----------:|-----------:|------:|
        | 'Express Hello 50' |     8.253 us |  0.1036 us |  0.0969 us | 0.003 |
        | 'Scriban Hello 50' | 2,831.377 us | 29.7322 us | 27.8115 us | 1.000 |
        */

        private readonly ExpressTemplateManager expressTemplateManager;
        private readonly Template scribanTemplate;
        private readonly RazorTemplatePage _razorTemplate;
        public RenderSimpleTemplate()
        {
            expressTemplateManager = new ExpressTemplateManager();
            scribanTemplate = Template.Parse("Hello {{name}}!");
            _razorTemplate = RazorBuilder.Compile(@"Hello @Model.Name!");
        }
        [Benchmark(Description = "Express Hello 50")]
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

        [Benchmark(Description = "Scriban Hello 50", Baseline = true)]
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

        [Benchmark(Description = "Razor Hello 50")]
        public string RazorHello50()
        {
            var writer = new StringWriter();
            for (int i = 0; i < 50; i++)
            {
                dynamic expando = new ExpandoObject();
                expando.Name = "World";
                _razorTemplate.Output = writer;
                _razorTemplate.Model = expando;
                _razorTemplate.Execute();
            }
            return writer.ToString();
        }
    }
}
