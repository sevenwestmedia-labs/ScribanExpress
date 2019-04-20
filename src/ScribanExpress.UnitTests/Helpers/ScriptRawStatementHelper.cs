using Scriban.Parsing;
using Scriban.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.UnitTests.Helpers
{
    internal class ScriptRawStatementHelper
    {
        internal static ScriptRawStatement CreateScriptRawStatement(string text)
        {
            return new ScriptRawStatement() { Text = text, Span = new SourceSpan { Start = new TextPosition(0, 0, 0), End = new TextPosition(text.Length -1, 0, 0) } };
        }
    }
}
