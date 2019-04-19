using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.UnitTests.Models
{
    public class Company
    {
        public string Title { get; set; }
        public string GetCompanyName(bool toUpper) {
                return toUpper ? Title.ToUpper() : Title;
        }
    }
}
