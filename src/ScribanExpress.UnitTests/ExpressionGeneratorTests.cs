using Microsoft.Extensions.Logging.Abstractions;
using Scriban;
using Scriban.Parsing;
using Scriban.Syntax;
using ScribanExpress.UnitTests.Helpers;
using ScribanExpress.UnitTests.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace ScribanExpress.UnitTests
{
    public class ExpressionGeneratorTests
    {
        Person person;
        public ExpressionGeneratorTests()
        {
            person = new Person()
            {
                FirstName = "Billy",
                Age = 23,
                Company = new Company { Title = "compname" }
            };
        }

        [Fact]
        public void RawTextBlock()
        {
            var templateText = @"This is a World from scriban!";
            var template = Template.Parse(templateText, null, null, null);

            var result = new StatementGenerator(new NullLogger<StatementGenerator>()).Generate<string, object>(template.Page.Body);

            var functor = result.Compile();
            var sb = new StringBuilder();
            functor(sb, "doesn'tm atter", null);

            Assert.Equal("This is a World from scriban!", sb.ToString());
        }



        [Fact]
        public void StringProperty()
        {
            var templateText = @"This is a {{ Length }} World from scriban!";
            var template = Template.Parse(templateText, null, null, null);

            var result = new StatementGenerator(new NullLogger<StatementGenerator>()).Generate<string, object>(template.Page.Body);

            var functor = result.Compile();
            var sb = new StringBuilder();
            functor(sb, "Hello", null);

            Assert.Equal("This is a 5 World from scriban!", sb.ToString());
        }

        [Fact]
        public void NamedStringProperty()
        {
            var personWrapper = new { person };

            var templateText = @"{{ person.FirstName }}";
            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(personWrapper, template.Page.Body);

            var functor = result.Compile();

            var sb = new StringBuilder();
            functor(sb, personWrapper, null);

            Assert.Equal("Billy", sb.ToString());
        }

        [Fact]
        public void NamedProperty_NotSet()
        {
            var person = new { person = new Person() };

            var templateText = @"{{ person.FirstName }}";
            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(person, template.Page.Body);

            var functor = result.Compile();

            var sb = new StringBuilder();
            functor(sb, person, null);

            Assert.Equal("", sb.ToString());
        }



        [Fact]
        public void NamedNumberProperty()
        {
            var personWrapper = new { person };

            var templateText = @"{{ person.Age }}";
            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(personWrapper, template.Page.Body);
            var functor = result.Compile();



            var sb = new StringBuilder();
            functor(sb, personWrapper, null);

            Assert.Equal("23", sb.ToString());
        }

        [Fact]
        public void ChainedProperty()
        {
            var personWrapper = new { person };

            var templateText = @"{{ person.Company.Title }}";
            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(personWrapper, template.Page.Body);

            var functor = result.Compile();

            var sb = new StringBuilder();
            functor(sb, personWrapper, null);

            Assert.Equal("compname", sb.ToString());
        }


        [Fact(Skip = "Should display a proper error or just return a space")]
        public void Pipeline()
        {
            var personWrapper = new { person };

            // string.downcase  doesn't exist so we should have a good error
            var templateText = @"{{ person.FirstName | string.downcase }}"; 
            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(personWrapper, template.Page.Body);

            var functor = result.Compile();

            var sb = new StringBuilder();
            functor(sb, personWrapper, null);

            Assert.Equal("billy", sb.ToString());
        }

        [Fact]
        public void Method_WithParameters_OnProperty()
        {
            var personWrapper = new { person };

            var templateText = @"blah blah {{ person.Age.ToString ""C"" }}";
            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(personWrapper, template.Page.Body);

            var functor = result.Compile();

            var sb = new StringBuilder();
            functor(sb, personWrapper, null);

            Assert.Equal("blah blah $23.00", sb.ToString());
        }


        //[Fact]
        //public void Pipeline_Multiple()
        //{
        //    var person = new { person = new Person() { FirstName = "Billy" } };

        //    //var templateText = @"This is a {{ text }} World from scriban!";
        //    var templateText = @"blah blah {{ person.FirstName | string.downcase | Length }}";
        //    var template = Template.Parse(templateText, null, null, null);

        //    var result = AnonGenerate(person, template.Page.Body);

        //    var functor = result.Compile();

        //    var stringResult = functor(person);

        //    // Assert.Equal(@"x => ""This is a Hello World from scriban!""", result.ToString());
        //    Assert.Equal("blah blah billy", stringResult);
        //}


        [Fact]
        public void Multi_TopLevelObjects()
        {
            Person oneperson = new Person() { FirstName = "Billy" };
            Person twoperson = new Person() { FirstName = "Bob" };
            var templateText = @"{{ oneperson.FirstName  }} {{ twoperson.FirstName }}"; //in our case it might be downcase
            var template = Template.Parse(templateText, null, null, null);

            var anon = new { oneperson, twoperson };

            var result = AnonGenerate(anon, template.Page.Body);

            var functor = result.Compile();
            var sb = new StringBuilder();
            functor(sb,anon, null);

            Assert.Equal("Billy Bob", sb.ToString());
        }

        public Expression<Action<StringBuilder,T, object>> AnonGenerate<T>(T value, ScriptBlockStatement scriptBlockStatement)
        {
            return Factory.CreateStatementGenerator().Generate<T, object>(scriptBlockStatement);
        }

    }
}
