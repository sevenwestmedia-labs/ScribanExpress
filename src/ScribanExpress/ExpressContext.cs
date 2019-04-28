using Scriban.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress
{
    public class ExpressContext
    {
        public ExpressContext()
        {
            this.Messages = new List<LogMessage>();
        }
        
        public List<LogMessage> Messages { get; }
    }
}
