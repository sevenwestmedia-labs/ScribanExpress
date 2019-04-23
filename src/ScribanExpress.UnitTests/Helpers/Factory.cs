using Microsoft.Extensions.Logging.Abstractions;
using ScribanExpress.Abstractions;
using ScribanExpress.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.UnitTests.Helpers
{
    public class Factory
    {
        public static IExpressTemplateManager CreateExpressTemplateManager()
        {
            return new ExpressTemplateManager<StandardLibrary>(new NullLogger<ExpressTemplateManager<StandardLibrary>>(), new StandardLibrary(), new StatementGenerator(new NullLogger<StatementGenerator>()));
        }

        public static StatementGenerator CreateStatementGenerator()
        {
            return new StatementGenerator(new NullLogger<StatementGenerator>());
        }
    }
}
