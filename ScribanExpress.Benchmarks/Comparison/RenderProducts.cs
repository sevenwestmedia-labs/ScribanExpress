using BenchmarkDotNet.Attributes;
using Scriban;
using Scriban.Runtime;
using ScribanExpress.Benchmarks.Comparison.ThirdParty.Razor;
using ScribanExpress.Benchmarks.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace ScribanExpress.Benchmarks.Comparison
{
    public class RenderProducts
    {
        private readonly List<Product> products;

        private readonly ExpressTemplateManager expressTemplateManager;
        private readonly Template scribanTemplate;
        private readonly RazorTemplatePage _razorTemplate;

        private const string Lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";

        private const string ScribanTemplateText = @"
<ul id='products'>
  {{ for product in products }}
    <li>
      <h2>{{ product.name }}</h2>
           Price: {{ product.price }}
           {{ product.description | string.truncate 15 }}
    </li>
  {{ end }}
</ul>
";

        public const string TestTemplateRazor = @"
<ul id='products'>
   @foreach(dynamic product in Model.products)
    {
    
    <li>
      <h2>@product.Name</h2>
           Only @product.Price
           @Model.truncate(product.Description, 15)
    </li>
   }
</ul>
";
        public RenderProducts()
        {
            const int ProductCount = 500;
            products = new List<Product>(ProductCount);
            for (int i = 0; i < ProductCount; i++)
            {
                var product = new Product("Name" + i, i, Lorem);
                products.Add(product);
            }

            expressTemplateManager = new ExpressTemplateManager();
            scribanTemplate = Template.Parse(ScribanTemplateText);
            _razorTemplate = RazorBuilder.Compile(TestTemplateRazor);
        }


        [Benchmark()]
        public string ExpressProducts500()
        {
            return expressTemplateManager.Render(ScribanTemplateText, new { products });
        }

        [Benchmark( Baseline = true)]
        public string ScribanProducts500()
        {
            return scribanTemplate.Render(new { products });
        }

        [Benchmark()]
        public string RazorProducts500()
        {
            dynamic expando = new ExpandoObject();
            expando.products = products;
            expando.truncate = new Func<string, int, string>((text, length) => Scriban.Functions.StringFunctions.Truncate(text, length));
            // why here?
            _razorTemplate.Output = new StringWriter();
            _razorTemplate.Model = expando;
            _razorTemplate.Execute();
            var result = _razorTemplate.Output.ToString();
            return result;
        }
    }
}
