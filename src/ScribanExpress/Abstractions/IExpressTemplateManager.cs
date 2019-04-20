using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.Abstractions
{
    public interface IExpressTemplateManager
    {
        string Render<T>(string templateText, T value);
    }
}
