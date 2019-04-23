using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging.Abstractions;
using Scriban;
using ScribanExpress.Benchmarks.Comparison.ThirdParty.Razor;
using ScribanExpress.Functions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace ScribanExpress.Benchmarks.Comparison
{
    public class RenderSimpleTemplate
    {
        /* 2019-04-23 Concat cacheKey
        |             Method |         Mean |      Error |    StdDev | Ratio |
        |------------------- |-------------:|-----------:|----------:|------:|
        | 'Express Hello 50' |     9.684 us |  0.0548 us | 0.0512 us | 0.004 |
        | 'Scriban Hello 50' | 2,752.831 us | 10.6530 us | 9.9648 us | 1.000 |
        |   'Razor Hello 50' |    20.551 us |  0.0679 us | 0.0636 us | 0.007 |
        */
        /*  2019-04-23 interpolation CacheKey
        |             Method |        Mean |      Error |     StdDev | Ratio |
        |------------------- |------------:|-----------:|-----------:|------:|
        | 'Express Hello 50' |    14.57 us |  0.0633 us |  0.0592 us | 0.005 |
        | 'Scriban Hello 50' | 2,785.09 us | 12.3685 us | 11.5695 us | 1.000 |
        |   'Razor Hello 50' |    20.72 us |  0.1865 us |  0.1744 us | 0.007 |
        */
        /* 2019-04-23 ConcurrentDictionary.GetOrAdd
        |             Method |         Mean |      Error |     StdDev | Ratio |
        |------------------- |-------------:|-----------:|-----------:|------:|
        | 'Express Hello 50' |     6.459 us |  0.0337 us |  0.0315 us | 0.002 |
        | 'Scriban Hello 50' | 2,769.453 us | 13.9965 us | 13.0923 us | 1.000 |
        |   'Razor Hello 50' |    20.868 us |  0.0627 us |  0.0586 us | 0.008 |
        */
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

        private readonly ExpressTemplateManager<StandardLibrary> expressTemplateManager;
        private readonly Template scribanTemplate;
        private readonly RazorTemplatePage _razorTemplate;
        public RenderSimpleTemplate()
        {
            expressTemplateManager = new ExpressTemplateManager<StandardLibrary>(new NullLogger<ExpressTemplateManager<StandardLibrary>>(),new StandardLibrary());
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
