using Scriban;
using Scriban.Parsing;
using Scriban.Syntax;
using ScribanExpress.UnitTests.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace ScribanExpress.UnitTests.Statements
{
   public class ForTests
    {


        [Fact]
        public void For()
        {
            var itemWrapper = new { Name = "Tim", Products = new List<Product> { new Product { Name = "aaaaa" }, new Product { Name = "bbbbb" } } };

    string templateText = @"
  {{ for product in products }}
<li>
            {{ product.name }}
    
</li>
  {{ end }}
";

            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(itemWrapper, template.Page.Body);

            var functor = result.Compile();

            var sb = new StringBuilder();
            functor(sb, itemWrapper, null);

            sb.ToString().ShouldContain("aaaaa");
            sb.ToString().ShouldContain("bbbbb");
        }

        [Fact]
        public void For_Nested()
        {

            var itemWrapper = new { Name = "Tim", Products = new List<Product> { new Product { Name = "a" }, new Product { Name = "b" } }, Items = new List<string> { "one","two","three" }   };

            string templateText = @"
{{- for item in Items -}}
      {{-  for product in products -}}
        {{- product.name -}}
    {{- end -}}  |
{{- end -}}";
      
            var template = Template.Parse(templateText, null, null,null);

            var result = AnonGenerate(itemWrapper, template.Page.Body);

            var functor = result.Compile();

            var sb = new StringBuilder();
            functor(sb, itemWrapper, null);

            sb.ToString().ShouldBe("ab|ab|ab|");
            
        }

        [Fact]
        public void For_Nested_WithSameName()
        {

            var itemWrapper = new { Name = "Tim", Products = new List<Product> { new Product { Name = "a" }, new Product { Name = "b" } }, Items = new List<string> { ".", ".", "." } };

            string templateText = @"
{{- for product in Items -}}
   {{-  for product in products -}} 
        {{ product.name -}}
    {{- end -}}
 {{- product -}} 
{{- end -}}";

            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(itemWrapper, template.Page.Body);

            var functor = result.Compile();

            var sb = new StringBuilder();
            functor(sb, itemWrapper, null);

            sb.ToString().ShouldBe("ab.ab.ab.");

        }



        public Expression<Action<StringBuilder, T, object>> AnonGenerate<T>(T value, ScriptBlockStatement scriptBlockStatement)
        {
            return new StatementGenerator().Generate<T, object>(scriptBlockStatement);
        }

    }
}
