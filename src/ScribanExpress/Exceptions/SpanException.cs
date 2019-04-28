using System;
using System.Collections.Generic;
using System.Text;
using Scriban.Parsing;

namespace ScribanExpress.Exceptions
{
    public class SpanException : Exception
    {
        public SpanException(string message, SourceSpan span)
            :base(message)
        {
            this.Span = span;
        }
        public SourceSpan Span { get; }
    }
}
