using Microsoft.Extensions.Logging.Abstractions;
using Scriban.Syntax;
using ScribanExpress.Abstractions;
using ScribanExpress.Functions;
using ScribanExpress.UnitTests.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ScribanExpress.UnitTests.Helpers
{
    public class Factory
    {
        public static IExpressTemplateManager CreateExpressTemplateManager()
        {
            return new ExpressTemplateManager<RootLibary>(new NullLogger<ExpressTemplateManager<RootLibary>>(), new RootLibary(), new StatementGenerator(new NullLogger<StatementGenerator>()));
        }

        public static StatementGenerator CreateStatementGenerator()
        {
            return new StatementGenerator(new NullLogger<StatementGenerator>());
        }

        public static Expression<Action<StringBuilder, T, object>> AnonGenerate<T>(T value, ScriptBlockStatement scriptBlockStatement)
        {
            return Factory.CreateStatementGenerator().Generate<T, object>(new ExpressContext(), scriptBlockStatement);
        }

    }
}
