using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScribanExpress.Demo.Models
{
    public class Blog
    {
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
    }
}
